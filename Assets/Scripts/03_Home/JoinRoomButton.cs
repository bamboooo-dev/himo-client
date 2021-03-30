using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinRoomButton : MonoBehaviour
{
  public void OnClickJoinRoomButton()
  {
    AudioManager.GetInstance().PlaySound(0);
    SceneManager.LoadScene("JoinRoom");
  }

}
