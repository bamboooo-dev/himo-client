using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToButton : MonoBehaviour
{
  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto1");
  }
}
