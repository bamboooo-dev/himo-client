using System;
using UnityEngine;
using WebSocketSharp;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public partial class GroupResponse
{
  public int subscribers;
}

public class WaitingRoomManager : MonoBehaviour
{
  public string channelName;
  public int maxNum;
  public Text maxSubscribersText;
  public Text channelNameText;
  public WebSocket ws;

  public bool isHost;

  void Start()
  {
    Debug.Log(channelName);
    ws = new WebSocket("ws://localhost:8000/");
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
    maxSubscribersText.text = maxNum.ToString();
    channelNameText.text = channelName;
  }

  private float timeLeft;
  public Text subscribersText;
  async void Update()
  {
    timeLeft -= Time.deltaTime;
    if (timeLeft <= 0.0)
    {
      timeLeft = 1.0f;
      GroupResponse response = await GetRequestAsync();
      subscribersText.text = response.subscribers.ToString();
      if (response.subscribers == maxNum)
      {
        if (isHost)
        {
          await StartRoomRequest();
        }
        SceneManager.LoadScene("GameField");
      }
    }

  }

  private async UniTask<GroupResponse> GetRequestAsync()
  {
    string url = "http://localhost:3000/group";
    var request = UnityWebRequest.Get(url);
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

  private async UniTask<GroupResponse> StartRoomRequest()
  {
    string url = "http://localhost:3000/group";
    var request = UnityWebRequest.Get(url);
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