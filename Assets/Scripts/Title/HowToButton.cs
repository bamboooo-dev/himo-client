using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToButton : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("HowTo");
  }
}
