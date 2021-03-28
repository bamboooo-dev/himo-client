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
  [SerializeField] private VotePlayer player;
  [SerializeField] private GameObject votePlayerParent;
  [SerializeField] private Sprite[] sprites;
  void Start()
  {
    // DEBUG
    // Cycle.names = new string[] { "a", "b", "c", "d", "e" };

    Cycle.mvpCount = new int[Cycle.names.Length];
    Cycle.mwpCount = new int[Cycle.names.Length];

    InstantiatePlayers(Cycle.names);
    ws = SetupWebSocket();
    ws.Connect();
  }

  async void Update()
  {
    if (PlayerStatus.isHost && (Cycle.mvpCount.Sum() == Cycle.names.Length))
    {
      var mvpIndex = Cycle.mvpCount.Select((x, i) => new { x, i })
    .Aggregate((max, xi) => xi.x > max.x ? xi : max).i;
      var mwpIndex = Cycle.mwpCount.Select((x, i) => new { x, i })
    .Aggregate((min, xi) => xi.x < min.x ? xi : min).i;
      Debug.Log(mvpIndex);
      Debug.Log(mwpIndex);
      await PostVoteResult(mvpIndex, mwpIndex);
    }
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
    if (!message.type.Equals("vote") & !message.type.Equals("voteResult")) return;
    if (message.type.Equals("vote") & PlayerStatus.isHost)
    {
      Cycle.mvpCount[message.mvpIndex]++;
      Cycle.mwpCount[message.mwpIndex]++;
    }
    else if (message.type.Equals("voteResult"))
    {
      Cycle.mvpIndex = message.mvpIndex;
      Cycle.mwpIndex = message.mwpIndex;
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
      var _player = Instantiate(player, new Vector3(x, 150, 0), Quaternion.identity);
      _player.transform.SetParent(votePlayerParent.transform.transform, false);
      _player.transform.Find("Name").GetComponent<Text>().text = names[i];

      Color newCol;
      ColorUtility.TryParseHtmlString(colors[i], out newCol);
      _player.transform.Find("Name").GetComponent<Text>().color = newCol;
      _player.transform.Find("EmoImage").GetComponent<Image>().sprite = sprites[i];
    }
  }

  private IEnumerator PostVoteResult(int mvpIndex, int mwpIndex)
  {
    VoteMessage message = new VoteMessage("voteResult", mvpIndex, mwpIndex);
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

  private void MiddleResultSceneLoaded(Scene next, LoadSceneMode mode)
  {
    var middleResultSceneManager = GameObject.FindWithTag("MiddleResultSceneManager").GetComponent<MiddleResultSceneManager>();
    middleResultSceneManager.result = result;

    SceneManager.sceneLoaded -= MiddleResultSceneLoaded;
  }
}
