using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public void OnClickPlayButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    string tokenPath = Application.persistentDataPath + "/access-token.jwt";

    if (File.Exists(tokenPath))
    {
      SceneManager.LoadScene("Home");
    }
    else
    {
      SceneManager.LoadScene("Nickname");
    }
  }
}
