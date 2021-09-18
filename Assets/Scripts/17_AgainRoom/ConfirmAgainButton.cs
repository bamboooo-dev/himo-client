using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;


[Serializable]
public partial class UpdateRoomResponse
{
  public string message;
  public string channel_name;
  public int max_num;
  public Theme[] themes;
}

public class ConfirmAgainButton : MonoBehaviour
{
  private UpdateRoomResponse response;

  void Start()
  {
    // DEBUG
    // RoomStatus.channelName = "cbd30a4";
  }

  public async void OnClickConfirmAgainRoomButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    try
    {
      await PostRequestAsync();
      RoomStatus.channelName = response.channel_name;
      RoomStatus.maxNum = response.max_num;
      RoomStatus.themes = response.themes;
      RoomStatus.cycleIndex = 0;
      PlayerStatus.isHost = true;
      SaveThemes(response.themes);
      await PostAgainAsync();
      SceneManager.LoadScene("WaitingRoom");
    }
    catch (UnauthorizedException)
    {
      ShowDialog("認証に失敗しました");
    }
    catch (UnityWebRequestException)
    {
      ShowDialog("通信に失敗しました");
    }
    catch (InvalidOperationException)
    {
      ShowDialog("通信に失敗しました");
    }
  }

  [SerializeField] private Dropdown categoryDropdown;
  private IEnumerator PostRequestAsync()
  {
    var updateRoomRequest = new UpdateRoomRequest();
    updateRoomRequest.theme_ids = Theme.RandomThemeIDs(categoryDropdown.value);
    updateRoomRequest.channel_name = RoomStatus.channelName;
    string myjson = JsonUtility.ToJson(updateRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
    var request = new UnityWebRequest(Url.Update(), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("Authorization", Token.getToken());
    yield return request.SendWebRequest();
    if (request.responseCode == 401)
    {
      throw new UnauthorizedException();
    }
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      response = JsonUtility.FromJson<UpdateRoomResponse>(request.downloadHandler.text);
    }
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

  // ダイアログを追加する親の GameObject
  [SerializeField] private GameObject parent = default;
  // 表示するダイアログ
  [SerializeField] private Dialog dialog = default;

  public void ShowDialog(string message)
  {
    // 生成してCanvasの子要素に設定
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(parent.transform, false);
    _dialog.transform.Find("Image_DialogBody").Find("Message").GetComponent<Text>().text = message;
    // ボタンが押されたときのイベント処理
    _dialog.FixDialog = result => Debug.Log(result);
  }

  private void SaveThemes(Theme[] themes)
  {
    string dirPath = SavePath.TmpDir(RoomStatus.channelName);
    if (!Directory.Exists(dirPath))
    {
      Directory.CreateDirectory(dirPath);
    }
    File.WriteAllText(dirPath + "themes.json", JsonUtility.ToJson(themes));
  }
}
