using UnityEngine;
using UnityEngine.SceneManagement;

public class Goto5 : MonoBehaviour
{
  void Start() { }

  void Update() { }

  public void OnClickHowTo()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("Howto5");
  }
}
