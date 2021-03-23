using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Cysharp.Threading.Tasks;

public class ConfirmJoinRoomButton : MonoBehaviour
{
  public InputField inputField;
  [SerializeField] private GameObject parent = default;
  [SerializeField] private Dialog dialog = default;

  // Start is called before the first frame update
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public async void OnClickConfirmJoinRoomButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    await PostRequestAsync();
  }

  private IEnumerator PostRequestAsync()
  {
    var enterRoomRequest = new EnterRoomRequest();
    enterRoomRequest.channel_name = inputField.text;
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
      ShowDialog("部屋が見つかりませんでした");
    }
    else if (request.isHttpError || request.isNetworkError)
    {
      ShowDialog("通信に失敗しました");
    }
    else
    {
      PlayerStatus.isHost = false;
      RoomStatus.channelName = inputField.text;
      SceneManager.LoadScene("WaitingRoom");
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
