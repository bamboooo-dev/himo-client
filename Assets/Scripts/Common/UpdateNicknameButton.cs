using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateNicknameButton : MonoBehaviour
{
  public void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("UpdateNickname");
  }
}
