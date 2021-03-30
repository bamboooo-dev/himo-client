using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;


[Serializable]
public partial class CreateRoomResponse
{
  public string message;
  public string channel_name;
  public int max_num;
  public Theme[] themes;
}

public class ConfirmCreateRoomButton : MonoBehaviour
{
  private CreateRoomResponse response;

  void Start() { }

  void Update() { }

  public async void OnClickConfirmCreateRoomButton()
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
      SceneManager.LoadScene("WaitingRoom");
    }
    catch (UnityWebRequestException)
    {
      ShowDialog();
    }
    catch (InvalidOperationException)
    {
      ShowDialog();
    }
  }

  [SerializeField] private Dropdown dropdownComponent;
  private IEnumerator PostRequestAsync()
  {
    var createRoomRequest = new CreateRoomRequest();
    createRoomRequest.max_num = Int32.Parse(dropdownComponent.options[dropdownComponent.value].text.ToString());
    createRoomRequest.theme_ids = RandomThemeIDs();
    string myjson = JsonUtility.ToJson(createRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
    var request = new UnityWebRequest(Url.Room(), "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("Authorization", Token.getToken());
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
    else
    {
      response = JsonUtility.FromJson<CreateRoomResponse>(request.downloadHandler.text);
    }
  }

  private int[] RandomThemeIDs()
  {
    int start = 1;
    int end = 40;
    int count = 3;
    int[] ids = new int[count];

    List<int> numbers = new List<int>();

    for (int i = start; i <= end; i++)
    {
      numbers.Add(i);
    }

    for (int i = 0; i < count; i++)
    {

      int index = UnityEngine.Random.Range(0, numbers.Count);
      int ransu = numbers[index];
      ids[i] = ransu;
      numbers.RemoveAt(index);
    }
    return ids;
  }


  // ダイアログを追加する親の GameObject
  [SerializeField] private GameObject parent = default;
  // 表示するダイアログ
  [SerializeField] private Dialog dialog = default;

  public void ShowDialog()
  {
    // 生成してCanvasの子要素に設定
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(parent.transform, false);
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
