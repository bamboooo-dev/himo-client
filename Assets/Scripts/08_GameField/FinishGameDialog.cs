using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using LitJson;

public class FinishGameDialog : MonoBehaviour
{
  [SerializeField] private FinishGameDialog finishGameDialog;
  public void OnCancel()
  {
    Destroy(this.gameObject);
  }

  public void OpenFinishGameDialog()
  {
    var dialog = Instantiate(finishGameDialog);
    var parent = GameObject.Find("Canvas");
    dialog.transform.SetParent(parent.transform, false);
  }

  public void OnClickFinishGameButton()
  {
    StartCoroutine(PostFinishGame());
  }

  private IEnumerator PostFinishGame()
  {
    GuessMessage message = new GuessMessage("finishGame", new int[Cycle.names.Length], Cycle.myIndex, RoomStatus.cycleIndex, Cycle.predicts);
    string json = JsonMapper.ToJson(message);
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
