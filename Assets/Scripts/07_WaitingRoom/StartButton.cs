using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections;

public class StartButton : MonoBehaviour
{
  public async void OnClickStartButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    await StartRoomRequest();
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
}
