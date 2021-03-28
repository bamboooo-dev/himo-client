using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

public class VoteButton : MonoBehaviour
{
  public async void OnClick()
  {
    // TODO 投票の結果を取得する処理を書く
    int mvpIndex = 0;
    int mwpIndex = 1;
    await PostVote(mvpIndex, mwpIndex);
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
