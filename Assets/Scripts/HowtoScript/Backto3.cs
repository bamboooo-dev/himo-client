using UnityEngine;
using UnityEngine.SceneManagement;

public class Backto3 : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto3");
  }
}
