using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OrderButton : MonoBehaviour
{
  [SerializeField] private Image successImage;
  [SerializeField] private Image failedImage;
  [SerializeField] private GameObject validationMessage;

  public void OnClick()
  {
    int playerIndex = PlayerPrefs.GetInt("radio");
    var (result, valid) = Judge(playerIndex);
    if (!valid)
    {
      validationMessage.SetActive(true);
      return;
    }
    else
    {
      validationMessage.SetActive(false);
    }
    StartCoroutine(PostOrder(result, playerIndex));
    PlayerPrefs.SetInt("radio", -1);
  }


  private (string, bool) Judge(int playerIndex)
  {
    if (playerIndex == -1)
    {
      return ("", false);
    }
    // 最後の要素だと "FIN" を返す
    if (Cycle.orderIndices.Length - 1 == Cycle.nextIndex)
    {
      return ("FIN", true);
    }
    else if (Cycle.orderIndices[Cycle.nextIndex] == playerIndex)
    {
      Cycle.nextIndex++;
      return ("OK", true);
    }
    else
    {
      return ("NG", true);
    }
  }

  private IEnumerator PostOrder(string result, int playerIndex)
  {
    OrderMessage message = new OrderMessage(result, playerIndex, RoomStatus.cycleIndex);
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
