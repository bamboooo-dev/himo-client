using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
  public void OnClickPlayButton()
  {
    AudioManager.GetInstance().PlaySound(0); 
    SceneManager.LoadScene("Nickname");
  }
}
