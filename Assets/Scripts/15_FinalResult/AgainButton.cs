using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

public class AgainButton : MonoBehaviour
{
  public async void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    await PostAgainAsync();
  }

  private IEnumerator PostAgainAsync()
  {
    FinalResultMessage message = new FinalResultMessage("again", Cycle.myIndex);
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
}
