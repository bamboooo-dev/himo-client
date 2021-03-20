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
  public GameObject startButton;

  public bool isHost;

  void Start()
  {
    Debug.Log(channelName);
    if (!isHost)
    {
      startButton.SetActive(false);
    }
    ws = new WebSocket(Url.WsSub(channelName));
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
    SceneManager.sceneLoaded += GameFieldSceneLoaded;
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
    var request = UnityWebRequest.Get(Url.Group(channelName));
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
    var request = UnityWebRequest.Get(Url.Start());
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

  private void GameFieldSceneLoaded(Scene next, LoadSceneMode mode)
  {
    var gameFieldManager = GameObject.FindWithTag("GameFieldManager").GetComponent<GameFieldManager>();
    gameFieldManager.playerCount = maxNum;
    gameFieldManager.isHost = isHost;

    SceneManager.sceneLoaded -= GameFieldSceneLoaded;
  }
}
