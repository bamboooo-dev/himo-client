using UnityEngine;
using UnityEngine.SceneManagement;

public class Backto6 : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto6");
  }
}
