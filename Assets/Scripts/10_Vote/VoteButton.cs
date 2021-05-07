using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class VoteButton : MonoBehaviour
{
  public void OnClick()
  {
    Button[] mvpBtns = GameObject.Find("VoteManager").GetComponent<VoteManager>().mvpBtns;
    Button[] mwpBtns = GameObject.Find("VoteManager").GetComponent<VoteManager>().mwpBtns;
    for (int i = 0; i < mvpBtns.Length; i++)
    {
      if (i == PlayerPrefs.GetInt("mvpIndex")) continue;
      mvpBtns[i].gameObject.SetActive(false);
    }
    for (int i = 0; i < mwpBtns.Length; i++)
    {
      if (i == PlayerPrefs.GetInt("mwpIndex")) continue;
      mwpBtns[i].gameObject.SetActive(false);
    }
    StartCoroutine(PostVote(PlayerPrefs.GetInt("mvpIndex"), PlayerPrefs.GetInt("mwpIndex")));
  }

  private IEnumerator PostVote(int mvpIndex, int mwpIndex)
  {
    VoteMessage message = new VoteMessage("vote", mvpIndex, mwpIndex, 0, 0, RoomStatus.cycleIndex, Cycle.myIndex);
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
