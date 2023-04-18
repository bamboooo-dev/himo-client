using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class FeedbackDialog : MonoBehaviour
{
  InputField inputField;
  [SerializeField] private Text validationMessage;
  [SerializeField] private Text thankMessage;
  [SerializeField] private GameObject twitterButton;
  
  void Start()
  {
    inputField = GameObject.Find("InputField").GetComponent<InputField>();
  }

  public void OnSend()
  {
    AudioManager.GetInstance().PlaySound(0);
    if (Validate(inputField.text))
    {
      StartCoroutine(PostFeedback(inputField.text));
      ShowMessage();
    }
    else
    {
      validationMessage.gameObject.SetActive(true);
    }
  }

  public void OnCancel()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }
  
  public void OnTwitter()
  {
    Application.OpenURL("https://twitter.com/bamboooo_inc");
  }

  public bool Validate(string text)
  {
    return text != "";
  }

  private IEnumerator PostFeedback(string text)
  {
    FeedbackData data = new FeedbackData(text);
    string json = JsonUtility.ToJson(data);
    byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
    var request = new UnityWebRequest(Env.slackURL, "POST");
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
      throw new InvalidOperationException(request.error);
    }
  }

  private void ShowMessage()
  {
    GameObject.Find("InputField").SetActive(false);
    GameObject.Find("SendButton").SetActive(false);
    validationMessage.gameObject.SetActive(false);
    thankMessage.gameObject.SetActive(true);
    twitterButton.SetActive(true);
  }
}

[Serializable]
public class FeedbackData
{
  public string text;

  public FeedbackData(string text)
  {
    this.text = text;
  }
}
