using System;
using UnityEngine;
using WebSocketSharp;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;

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
  public string channelName;
  public Text maxSubscribersText;
  public Text channelNameText;
  public WebSocket ws;
  public GameObject startButton;

  void Start()
  {
    Debug.Log(channelName);
    if (!PlayerStatus.isHost)
    {
      startButton.SetActive(false);
    }
    if (PlayerStatus.isHost)
    {
      ws = new WebSocket(Url.WsSub(channelName, RoomStatus.maxNum));
    }
    else
    {
      ws = new WebSocket(Url.WsSub(channelName));
    }
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };

    ws.OnMessage += (sender, e) =>
    {
      Debug.Log("WebSocket Data: " + e.Data);
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
    channelNameText.text = channelName;
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
    }
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private async UniTask<GroupResponse> GetRequestAsync()
  {
    var request = UnityWebRequest.Get(Url.Group(channelName));
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

}
