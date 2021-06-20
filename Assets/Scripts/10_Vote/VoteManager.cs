using System;
using System.Linq;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class VoteManager : MonoBehaviour
{
  public string channelName;
  public int maxNum;
  public bool isHost;
  private WebSocket ws;
  private VoteResult result;
  public Button[] mvpBtns;
  public Button[] mwpBtns;
  public Text messageText;
  public VotePlayer[] players;
  private bool connected;

  [SerializeField] private VotePlayer player;
  [SerializeField] private GameObject votePlayerParent;
  [SerializeField] private Sprite[] sprites;
  void Start()
  {
    // DEBUG
    // Cycle.names = new string[] { "a", "b", "c", "d", "e", "f" };
    // Cycle.numbers = new int[] { 20, 59, 100, 40, 10, 30 };
    // Cycle.predicts = new int[][]{
    //   new int[] { 20, 40, 80, 35, 11, 32 },
    //   new int[] { 22, 59, 90, 45, 15, 35 },
    //   new int[] { 30, 60, 95, 33, 22, 11 },
    //   new int[] { 40, 30, 70, 34, 20, 50 },
    //   new int[] { 10, 50, 82, 22, 10, 30 },
    //   new int[] { 25, 55, 83, 41, 11, 33 },
    // };

    Cycle.mvpCount = new int[Cycle.names.Length];
    for (int i = 0; i < Cycle.names.Length; ++i) { Cycle.mvpCount[i] = -1; }
    Cycle.mwpCount = new int[Cycle.names.Length];
    for (int i = 0; i < Cycle.names.Length; ++i) { Cycle.mwpCount[i] = -1; }

    players = new VotePlayer[Cycle.names.Length];
    InstantiatePlayers(Cycle.names);
    PlayerPrefs.SetInt("mvpIndex", 0);
    PlayerPrefs.SetInt("mwpIndex", 0);
    SetMVPBtns();
    SetMWPBtns();
    SetupWebSocket();
    InvokeRepeating("CheckWebSocketConnection", 5.0f, 5.0f);
  }

  private void SetupWebSocket()
  {
    ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };
    var context = SynchronizationContext.Current;
    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data, context);
    };
    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
    };
    ws.OnClose += (sender, e) =>
    {
      Debug.Log($"Websocket Close. StatusCode: {e.Code} Reason: {e.Reason}");
      if (e.Code == 1006) { ws.Connect(); }
    };
    ws.Connect();
  }

  private void CheckWebSocketConnection()
  {
    if (!connected)
    {
      try
      {
        ws.Close();
        SetupWebSocket();
      }
      catch (Exception e) { Debug.Log(e); }
    }
    connected = false;
    StartCoroutine(PostPing());
  }

  private IEnumerator PostPing()
  {
    VoteMessage message = new VoteMessage("ping", 0, 0, 0, 0, RoomStatus.cycleIndex, Cycle.myIndex);
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

  private void ProcessData(string data, SynchronizationContext context)
  {
    var message = JsonUtility.FromJson<VoteMessage>(data);
    if (message.cycleIndex != RoomStatus.cycleIndex) return;
    switch (message.type)
    {
      case "vote":
        context.Post(state =>
        {
          int index = Int32.Parse(state.ToString());
          players[index].transform.Find("VotedImage").gameObject.SetActive(true);
          if (index == Cycle.myIndex)
          {
            messageText.gameObject.SetActive(true);
            GameObject.Find("VoteButton").SetActive(false);
          }
        }, message.playerIndex);
        if (PlayerStatus.isHost)
        {
          Cycle.mvpCount[message.playerIndex] = message.mvpIndex;
          Cycle.mwpCount[message.playerIndex] = message.mwpIndex;
          var (isFin, mvpIndex, mwpIndex) = checkCount(Cycle.mvpCount, Cycle.mwpCount);
          if (!isFin) { return; }
          var (nearIndex, farIndex) = CalcScore();
          context.Post(state =>
          {
            StartCoroutine(PostVoteResult(mvpIndex, mwpIndex, nearIndex, farIndex));
          }, message);
        }
        break;

      case "voteResult":
        Cycle.mvpIndex = message.mvpIndex;
        Cycle.mwpIndex = message.mwpIndex;
        Cycle.nearIndex = message.nearIndex;
        Cycle.farIndex = message.farIndex;
        context.Post(state =>
        {
          SceneManager.LoadScene("VoteResult");
        }, message);
        break;

      case "ping":
        if (Cycle.myIndex != message.playerIndex) { return; }
        connected = true;
        break;

      default:
        break;
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
      players[i] = _player;
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
    VoteMessage message = new VoteMessage("voteResult", mvpIndex, mwpIndex, nearIndex, farIndex, RoomStatus.cycleIndex, 0);
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

  private (bool, int, int) checkCount(int[] mvpIndices, int[] mwpIndices)
  {
    int[] mvpCount = new int[Cycle.names.Length];
    int[] mwpCount = new int[Cycle.names.Length];
    for (int i = 0; i < Cycle.names.Length; i++)
    {
      if (mvpIndices[i] == -1) { return (false, 0, 0); }
      mvpCount[mvpIndices[i]]++;
      mwpCount[mwpIndices[i]]++;
    }
    int mvpIndex = Array.IndexOf(mvpCount, mvpCount.Max());
    int mwpIndex = Array.IndexOf(mwpCount, mwpCount.Max());

    return (true, mvpIndex, mwpIndex);
  }
}
