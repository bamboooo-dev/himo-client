using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

public class AgainDialog : MonoBehaviour
{
  [SerializeField] private Dialog dialog = default;
  [SerializeField] private GameObject parent = default;
  private JoinRoomResponse response;

  public async void OnClickAgainButton()
  {
    AudioManager.GetInstance().PlaySound(0);

    try
    {
      await PostRequestAsync(RoomStatus.channelName);
      RoomStatus.maxNum = response.max_num;
      RoomStatus.themes = response.themes;
      RoomStatus.cycleIndex = 0;
      PlayerStatus.isHost = false;
      SceneManager.LoadScene("WaitingRoom");
    }
    catch (RoomNotFoundException)
    {
      ShowDialog("部屋が見つかりませんでした");
    }
    catch (InvalidOperationException)
    {
      ShowDialog("通信に失敗しました");
    }
  }

  public void OnClickCancelButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    RoomStatus.finished = true;
    SceneManager.LoadScene("Home");
  }

  private IEnumerator PostRequestAsync(string channelName)
  {
    var enterRoomRequest = new EnterRoomRequest();
    enterRoomRequest.channel_name = channelName;
    string requestJson = JsonUtility.ToJson(enterRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(requestJson);
    var request = new UnityWebRequest(Url.Enter(), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("Authorization", Token.getToken());
    yield return request.SendWebRequest();
    if (request.responseCode == 404)
    {
      throw new RoomNotFoundException();
    }
    else if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      response = JsonUtility.FromJson<JoinRoomResponse>(request.downloadHandler.text);
    }
  }

  public void ShowDialog(string message)
  {
    // 生成してCanvasの子要素に設定
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(parent.transform, false);
    _dialog.transform.Find("Image_DialogBody").Find("Message").GetComponent<Text>().text = message;
    // ボタンが押されたときのイベント処理
    _dialog.FixDialog = result => Debug.Log(result);
  }
}
