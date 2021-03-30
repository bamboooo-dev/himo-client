using System;
using System.Linq;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using Cysharp.Threading.Tasks;

public class VoteManager : MonoBehaviour
{
  public string channelName;
  public int maxNum;
  public bool isHost;
  private WebSocket ws;
  private VoteResult result;
  public Button[] mvpBtns;
  public Button[] mwpBtns;

  [SerializeField] private VotePlayer player;
  [SerializeField] private GameObject votePlayerParent;
  [SerializeField] private Sprite[] sprites;
  void Start()
  {
    // DEBUG
    // Cycle.names = new string[] { "a", "b", "c" };
    // Cycle.numbers = new int[] { 20, 59, 100 };

    Cycle.mvpCount = new int[Cycle.names.Length];
    Cycle.mwpCount = new int[Cycle.names.Length];

    InstantiatePlayers(Cycle.names);
    PlayerPrefs.SetInt("mvpIndex", 0);
    PlayerPrefs.SetInt("mwpIndex", 0);
    SetMVPBtns();
    SetMWPBtns();
    ws = SetupWebSocket();
    ws.Connect();
  }

  private WebSocket SetupWebSocket()
  {
    ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    var context = SynchronizationContext.Current;
    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data, context);
    };
    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
    };
    return ws;
  }

  private void ProcessData(string data, SynchronizationContext context)
  {
    var message = JsonUtility.FromJson<VoteMessage>(data);
    if ((!message.type.Equals("vote") & !message.type.Equals("voteResult")) | message.cycleIndex != RoomStatus.cycleIndex) return;
    if (message.type.Equals("vote") & PlayerStatus.isHost)
    {
      Cycle.mvpCount[message.mvpIndex]++;
      Cycle.mwpCount[message.mwpIndex]++;
      if (Cycle.mvpCount.Sum() == Cycle.names.Length)
      {
        var mvpIndex = Array.IndexOf(Cycle.mvpCount, Cycle.mvpCount.Max());
        var mwpIndex = Array.IndexOf(Cycle.mwpCount, Cycle.mwpCount.Max());
        var (nearIndex, farIndex) = CalcScore();
        context.Post(async state =>
        {
          await PostVoteResult(mvpIndex, mwpIndex, nearIndex, farIndex);
        }, message);
      }
    }
    else if (message.type.Equals("voteResult"))
    {
      Cycle.mvpIndex = message.mvpIndex;
      Cycle.mwpIndex = message.mwpIndex;
      Cycle.nearIndex = message.nearIndex;
      Cycle.farIndex = message.farIndex;
      Debug.Log("MVP Index: " + Cycle.mvpIndex);
      Debug.Log("MWP Index: " + Cycle.mwpIndex);
      Debug.Log("Near Index: " + Cycle.nearIndex);
      Debug.Log("Far Index: " + Cycle.farIndex);
      context.Post(state =>
      {
        SceneManager.LoadScene("VoteResult");
      }, message);
    }
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private void InstantiatePlayers(string[] names)
  {
    int count = names.Length;

    string[] colors = new string[] { "#018D50", "#0178C2", "#FD8016", "#C4453F", "#714A9B", "#C031A5" };

    mvpBtns = new Button[names.Length];
    mwpBtns = new Button[names.Length];

    for (int i = 0; i < count; ++i)
    {
      int x;
      if (count == 1)
      {
        x = 0;
      }
      else
      {
        x = -750 + i * 1500 / (count - 1);
      }
      var _player = Instantiate(player, new Vector3(x, -20, 0), Quaternion.identity);
      _player.transform.SetParent(votePlayerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];

      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];
      _player.transform.Find("Answer").Find("Text").GetComponent<Text>().text = Cycle.numbers[i].ToString();
      _player.transform.Find("Answer").Find("Text").GetComponent<Text>().color = newCol;

      // MVP・MWP ボタンの初期設定
      int tmp = i;
      var mvpButton = _player.transform.Find("MVPButton").GetComponent<Button>();
      var mwpButton = _player.transform.Find("MWPButton").GetComponent<Button>();
      mvpButton.onClick.AddListener(delegate { OnClickMVPButton(tmp); });
      mwpButton.onClick.AddListener(delegate { OnClickMWPButton(tmp); });
      mvpBtns[tmp] = mvpButton;
      mwpBtns[tmp] = mwpButton;
    }
  }

  void OnClickMVPButton(int tmp)
  {
    PlayerPrefs.SetInt("mvpIndex", tmp);
    SetMVPBtns();
  }

  void OnClickMWPButton(int tmp)
  {
    PlayerPrefs.SetInt("mwpIndex", tmp);
    SetMWPBtns();
  }

  private IEnumerator PostVoteResult(int mvpIndex, int mwpIndex, int nearIndex, int farIndex)
  {
    VoteMessage message = new VoteMessage("voteResult", mvpIndex, mwpIndex, nearIndex, farIndex, RoomStatus.cycleIndex);
    string json = JsonUtility.ToJson(message);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
    var request = new UnityWebRequest(Url.Pub(RoomStatus.channelName), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
  }

  private void SetMVPBtns()
  {
    int r = PlayerPrefs.GetInt("mvpIndex"), cnt = 0;
    foreach (Button b in mvpBtns)
    {
      b.interactable = (cnt != r); cnt++;
    }
  }

  private void SetMWPBtns()
  {
    int r = PlayerPrefs.GetInt("mwpIndex"), cnt = 0;
    foreach (Button b in mwpBtns)
    {
      b.interactable = (cnt != r); cnt++;
    }
  }

  private (int, int) CalcScore()
  {
    int nearIndex = 0;
    int nearLoss = 10000;
    int farIndex = 0;
    int farLoss = -10000;

    for (int i = 0; i < Cycle.names.Length; i++)
    {
      int loss = 0;
      for (int j = 0; j < Cycle.names.Length; j++)
      {
        loss += Math.Abs(Cycle.predicts[i][j] - Cycle.numbers[j]);
      }
      if (loss < nearLoss)
      {
        nearIndex = i;
        nearLoss = loss;
      }
      if (loss > farLoss)
      {
        farIndex = i;
        farLoss = loss;
      }
    }
    return (nearIndex, farIndex);
  }
}
