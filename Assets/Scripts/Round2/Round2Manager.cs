using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using Cysharp.Threading.Tasks;

public class Round2Manager : MonoBehaviour
{
  private WebSocket ws;
  void Start()
  {
    ws = new WebSocket(Url.WsSub(RoomStatus.channelName));
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
    };

    ws.OnMessage += (sender, e) =>
    {
      ProcessData(e.Data);
    };
    ws.Connect();
    // await StartRoomRequest();
  }

  void OnDestroy()
  {
    ws.Close();
    ws = null;
  }

  private void ProcessData(string data)
  {
    var response = JsonUtility.FromJson<StartRoomResponse>(data);
    Cycle.numbers = response.numbers;
    Cycle.orderIndices = SortIndices(Cycle.numbers);
    Cycle.names = response.names;
    Cycle.started = true;
  }

  private IEnumerator StartRoomRequest()
  {
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

  private int[] SortIndices(int[] numbers)
  {
    int[] indices = new int[numbers.Length];
    int[] sortedNumbers = numbers.OrderBy(x => x).ToArray();
    for (int i = 0; i < numbers.Length; i++)
    {
      indices[i] = Array.IndexOf(sortedNumbers, numbers[i]);
    }
    return indices;
  }
}
