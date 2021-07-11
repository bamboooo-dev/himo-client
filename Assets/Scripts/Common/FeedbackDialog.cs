using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FeedbackDialog : MonoBehaviour
{
  InputField inputField;
  void Start()
  {
    inputField = GameObject.Find("InputField").GetComponent<InputField>();
  }

  public void OnSend()
  {
    AudioManager.GetInstance().PlaySound(0);
    if (Validate(inputField.text))
    {
      Destroy(this.gameObject);
    }
    else
    {
      GameObject.Find("ValidationMessage").GetComponent<InputField>();
    }
  }

  public void OnCancel()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }

  public bool Validate(string text)
  {
    return text != "";
  }
}
