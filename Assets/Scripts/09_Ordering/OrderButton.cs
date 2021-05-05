using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class OrderButton : MonoBehaviour
{
  [SerializeField] private Image successImage;
  [SerializeField] private Image failedImage;

  public void OnClick()
  {
    int playerIndex = PlayerPrefs.GetInt("radio");
    string result = Judge(playerIndex);
    StartCoroutine(PostOrder(result, playerIndex));
    if (result == "OK")
    {
      successImage.gameObject.SetActive(true);
      GameObject.Find("OrderingManager").GetComponent<OrderingManager>().players[playerIndex].gameObject.SetActive(false);
      Invoke(nameof(EraseSuccessImage), 1.0f);
    }
    else if (result == "NG")
    {
      failedImage.gameObject.SetActive(true);
      Invoke(nameof(EraseFailedImage), 1.0f);
    }
    else if (result == "FIN")
    {
      successImage.gameObject.SetActive(true);
      Invoke(nameof(Finish), 1.0f);
    }
  }

  private void EraseSuccessImage()
  {
    successImage.gameObject.SetActive(false);
  }

  private void EraseFailedImage()
  {
    failedImage.gameObject.SetActive(false);
    SceneManager.LoadScene("Vote");
  }

  private void Finish()
  {
    successImage.gameObject.SetActive(false);
    SceneManager.LoadScene("Vote");
  }

  private string Judge(int playerIndex)
  {
    // 最後の要素だと "FIN" を返す
    if (Cycle.orderIndices.Length - 1 == Cycle.nextIndex)
    {
      return "FIN";
    }
    else if (Cycle.orderIndices[Cycle.nextIndex] == playerIndex)
    {
      Cycle.nextIndex++;
      return "OK";
    }
    else
    {
      return "NG";
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
