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
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

  }

  [Serializable]
  private class Data
  {
    public int id;
    public string title = "test";
    public string author = "monaka";
  }
  public void OnClickConfirmJoinRoomButton()
  {
    StartCoroutine(PostRequestAsync());
  }

  private IEnumerator PostRequestAsync()
  {
    string url = "http://localhost:3000/posts";
    var data = new Data();
    data.id = Int32.Parse(inputField.text);
    string myjson = JsonUtility.ToJson(data);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(myjson);
    var request = new UnityWebRequest(url, "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    if (request.responseCode == 201)
    {
      SceneManager.LoadScene("WaitingRoom");
    }
  }
}
