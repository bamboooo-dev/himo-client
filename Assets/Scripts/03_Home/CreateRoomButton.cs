using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateRoomButton : MonoBehaviour
{
  public void OnClickCreateRoomButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("CreateRoom");
  }

}
