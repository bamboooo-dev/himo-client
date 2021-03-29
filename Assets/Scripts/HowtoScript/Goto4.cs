using UnityEngine;
using UnityEngine.SceneManagement;

public class Goto4 : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto4");
  }
}
