using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
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
