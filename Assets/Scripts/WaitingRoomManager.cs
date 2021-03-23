using System;
using System.Linq;
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

  void Start()
  {
    Debug.Log(RoomStatus.channelName);
    if (!PlayerStatus.isHost)
    {
      startButton.SetActive(false);
    }
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
      Debug.Log("WebSocket Close");
    };

    ws.Connect();
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

      // Cycle の started が true ならゲーム開始
      if (Cycle.started)
      {
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

  private void ProcessData(string data)
  {
    var response = JsonUtility.FromJson<StartRoomResponse>(data);
    Cycle.numbers = response.numbers;
    Cycle.names = response.names;
    Cycle.started = true;
  }
}
