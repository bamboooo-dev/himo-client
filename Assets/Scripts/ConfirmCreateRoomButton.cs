using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class CreateRoomResponse
{
  public string message;
  public string channel_name;
  public int max_num;
  public int[] theme_ids;
}

public class ConfirmCreateRoomButton : MonoBehaviour
{
  private CreateRoomResponse response;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public async void OnClickConfirmCreateRoomButton()
  {
    try
    {
      SceneManager.sceneLoaded += WaitingRoomSceneLoaded;
      await PostRequestAsync();
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

  [Serializable]
  private class CreateRoomRequest
  {
    public int max_num;
    public int[] theme_ids;
  }

  [SerializeField] private Dropdown dropdownComponent;
  private IEnumerator PostRequestAsync()
  {
    string url = "http://localhost:3000/posts";
    var createRoomRequest = new CreateRoomRequest();
    createRoomRequest.max_num = Int32.Parse(dropdownComponent.options[dropdownComponent.value].text.ToString());
    createRoomRequest.theme_ids = RandomThemeIDs();
    string myjson = JsonUtility.ToJson(createRoomRequest);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
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
      response = JsonUtility.FromJson<CreateRoomResponse>(request.downloadHandler.text);
      SceneManager.LoadScene("WaitingRoom");
    }
  }

  private int[] RandomThemeIDs()
  {
    int start = 1;
    int end = 100;
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

  private void WaitingRoomSceneLoaded(Scene next, LoadSceneMode mode)
  {
    // シーン切り替え後のスクリプトを取得
    var waitingRoomManager = GameObject.FindWithTag("WaitingRoomManager").GetComponent<WaitingRoomManager>();

    // データを渡す処理
    waitingRoomManager.channelName = response.channel_name;
    waitingRoomManager.maxNum = response.max_num;

    // イベントから削除
    SceneManager.sceneLoaded -= WaitingRoomSceneLoaded;
  }

}
