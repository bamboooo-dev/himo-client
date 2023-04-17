using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateNicknameButton : MonoBehaviour
{
  public void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);

    SceneManager.LoadScene(File.Exists(SavePath.token) ? "UpdateNickname" : "Nickname");
  }
}
