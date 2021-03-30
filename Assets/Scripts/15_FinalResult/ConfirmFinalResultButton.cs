using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmFinalResultButton : MonoBehaviour
{
  public void OnClickConfirmFinalResultButton()
  {
    SceneManager.LoadScene("MiddleResult");
  }

}
