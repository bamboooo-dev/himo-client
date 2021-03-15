using UnityEngine;
using UnityEngine.SceneManagement;


public class ConfirmVoteRoomButton : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnClickConfirmVoteRoomButton()
  {
    SceneManager.LoadScene("MiddleResult");
  }

}
