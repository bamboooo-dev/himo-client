using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

public class VoteButton : MonoBehaviour
{
  public Text messageText;

  public async void OnClick()
  {
    this.gameObject.SetActive(false);
    messageText.gameObject.SetActive(true);
    await PostVote(PlayerPrefs.GetInt("mvpIndex"), PlayerPrefs.GetInt("mwpIndex"));
  }

  private IEnumerator PostVote(int mvpIndex, int mwpIndex)
  {
    VoteMessage message = new VoteMessage("vote", mvpIndex, mwpIndex);
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