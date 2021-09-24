using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
  public void OnClick()
  {
    StartCoroutine(PostReaction());
  }

  private IEnumerator PostReaction()
  {
    string spriteName = this.gameObject.transform.Find("EmoImage").GetComponent<Image>().sprite.name;
    int index = Int32.Parse(spriteName.Substring(3));
    GuessMessage message = new GuessMessage("reaction", null, Cycle.myIndex, RoomStatus.cycleIndex, null, reaction: (Reaction)(index + 1));
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
