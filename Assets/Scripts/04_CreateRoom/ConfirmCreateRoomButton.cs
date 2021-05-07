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

  [SerializeField] private Dropdown dropdownComponent;
  [SerializeField] private Dropdown categoryDropdown;
  private IEnumerator PostRequestAsync()
  {
    var createRoomRequest = new CreateRoomRequest();
    createRoomRequest.max_num = Int32.Parse(dropdownComponent.options[dropdownComponent.value].text.ToString());
    createRoomRequest.theme_ids = RandomThemeIDs(categoryDropdown.value);
    string myjson = JsonUtility.ToJson(createRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
    var request = new UnityWebRequest(Url.Room(), "POST");
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
      response = JsonUtility.FromJson<CreateRoomResponse>(request.downloadHandler.text);
    }
  }

  private int[] RandomThemeIDs(int categoryID)
  {
    int start = 0;
    int end = 0;
    switch (categoryID)
    {
      case 0: // おまかせ
        start = 1;
        end = 40;
        break;
      case 1: // エンジニア
        start = 41;
        end = 80;
        break;
      case 2: // 18禁
        start = 81;
        end = 120;
        break;
    }

    int count = 3;
    int[] ids = new int[count];

    List<int> numbers = new List<int>();

    for (int i = start; i <= end; i++)
    {
      numbers.Add(i);
    }
    UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
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
