using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnClickStartButton()
  {
    SceneManager.LoadScene("Gamefield");
  }
}
