using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConfirmJoinRoomButton : MonoBehaviour
{
  public InputField inputField;

  // Start is called before the first frame update
  void Start() { }

  // Update is called once per frame
  void Update() { }

  [Serializable]
  private class EnterRoomRequest
  {
    public string channel_name;
  }
  public void OnClickConfirmJoinRoomButton()
  {
    SceneManager.sceneLoaded += WaitingRoomSceneLoaded;
    StartCoroutine(PostRequestAsync());
  }

  private IEnumerator PostRequestAsync()
  {
    string url = "http://localhost:3000/posts";
    var enterRoomRequest = new EnterRoomRequest();
    enterRoomRequest.channel_name = inputField.text;
    string requestJson = JsonUtility.ToJson(enterRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(requestJson);
    var request = new UnityWebRequest(url, "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      SceneManager.LoadScene("WaitingRoom");
    }
  }

  private void WaitingRoomSceneLoaded(Scene next, LoadSceneMode mode)
  {
    // シーン切り替え後のスクリプトを取得
    var waitingRoomManager = GameObject.FindWithTag("WaitingRoomManager").GetComponent<WaitingRoomManager>();

    // データを渡す処理
    waitingRoomManager.channelName = inputField.text;

    // イベントから削除
    SceneManager.sceneLoaded -= WaitingRoomSceneLoaded;
  }
}
