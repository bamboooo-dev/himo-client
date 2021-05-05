using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using WebSocketSharp;

public class RoundManager : MonoBehaviour
{
  private WebSocket ws;
  private float step_time;
  private int count;
  void Start()
  {
    Debug.Log("Round Scene start");

    // DEBUG
    // RoomStatus.channelName = "263261a";

    SetupWebSocket();
    count = 0;
    if (PlayerStatus.isHost)
    {
      StartCoroutine(StartRoomRequest());
    }
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
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
    ws.OnClose += (sender, e) =>
    {
      Debug.Log($"Websocket Close. StatusCode: {e.Code} Reason: {e.Reason}");
      if (e.Code == 1006) { ws.Connect(); }
    };
    ws.Connect();
  }

  private void ProcessData(string data, SynchronizationContext context)
  {
    Debug.Log(data);
    context.Post(state =>
    {
      var response = JsonUtility.FromJson<StartRoomResponse>(data);
      if (response.type != "answer") return;

      // 以前のデータも流れてくるため今回のデータのみ採用する
      if (count < RoomStatus.cycleIndex)
      {
        count++;
        return;
      }
      Cycle.numbers = response.numbers;
      Cycle.names = response.names;
      SceneManager.LoadScene("CardCheck");
    }, data);
  }

  private IEnumerator StartRoomRequest()
  {
    yield return new WaitForSeconds(3);
    var startRoomRequest = new StartRoomRequest(RoomStatus.channelName);
    string myjson = JsonUtility.ToJson(startRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
    var request = new UnityWebRequest(Url.Start(), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("Authorization", Token.getToken());
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
  }
}
