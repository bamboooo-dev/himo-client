using UnityEngine;
using UnityEngine.SceneManagement;


public class ConfirmFinalResultButton : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnClickConfirmFinalResultButton()
  {
    SceneManager.LoadScene("MiddleResult");
  }

}
