using UnityEngine;
using UnityEngine.SceneManagement;


public class ConfirmMiddleResutlButton : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnClickConfirmMiddleResultButton()
  {
    SceneManager.LoadScene("Calculating");
  }

}
