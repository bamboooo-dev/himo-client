using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using Cysharp.Threading.Tasks;

[Serializable]
public partial class GroupResponse
{
  public int subscribers;
  public Limits limits;
}

[Serializable]
public class Limits
{
  public int subscribers;
}

public class WaitingRoomManager : MonoBehaviour
{
  public Text maxSubscribersText;
  public Text channelNameText;
  public WebSocket ws;
  public GameObject startButton;
  private bool connected;

  void Start()
  {
    Debug.Log("WaitingRoom scene started");
    Debug.Log("channelName: " + RoomStatus.channelName);
    RoomStatus.started = false;
    if (!PlayerStatus.isHost)
    {
      startButton.SetActive(false);
    }

    SetupWebSocket();
    InvokeRepeating("CheckWebSocketConnection", 5.0f, 5.0f);
    channelNameText.text = RoomStatus.channelName;
  }

  private float timeLeft;
  public Text nowSubscribersText;
  async void Update()
  {
    timeLeft -= Time.deltaTime;
    if (timeLeft <= 0.0)
    {
      timeLeft = 1.0f;
      GroupResponse response = await GetRequestAsync();
      nowSubscribersText.text = response.subscribers.ToString();
      maxSubscribersText.text = response.limits.subscribers.ToString();

      // POST /start が 2回送られる可能性があるので人数が揃ったことによるゲーム開始は行わないものとする
      // if (PlayerStatus.isHost & response.subscribers == response.limits.subscribers)
      // {
      //   startButton.GetComponent<StartButton>().OnClickStartButton();
      // }

      // RoomStatus の started が true ならゲーム開始
      if (RoomStatus.started)
      {
        // 各プレイヤーの持ち点は10点で開始する
        RoomStatus.points = Enumerable.Repeat<int>(10, Cycle.names.Length).ToArray();
        SceneManager.LoadScene("CardCheck");
      }
    }
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private async UniTask<GroupResponse> GetRequestAsync()
  {
    var request = UnityWebRequest.Get(Url.Group(RoomStatus.channelName));
    request.SetRequestHeader("Accept", "text/json");
    await request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException("failure.");
    }
    else
    {
      GroupResponse res = JsonUtility.FromJson<GroupResponse>(request.downloadHandler.text);
      return res;
    }
  }

  private void SetupWebSocket()
  {
    if (PlayerStatus.isHost)
    {
      ws = new WebSocket(Url.WsSub(RoomStatus.channelName, RoomStatus.maxNum));
    }
    else
    {
      ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    }
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };
    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data);
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
      catch (Exception e)
      {
        Debug.Log(e.ToString());
      }
    }
    connected = false;
    StartCoroutine(PostPing());
  }

  private IEnumerator PostPing()
  {
    StartRoomResponse message = new StartRoomResponse("ping");
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

  private void ProcessData(string data)
  {
    var message = JsonUtility.FromJson<StartRoomResponse>(data);
    switch (message.type)
    {
      case "answer":
        Cycle.numbers = message.numbers;
        Cycle.names = message.names;
        RoomStatus.started = true;
        break;
      case "ping":
        connected = true;
        break;
      default:
        break;
    }
  }
}
