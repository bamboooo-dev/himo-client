using UnityEngine;
using UnityEngine.SceneManagement;

public class VoteButton : MonoBehaviour
{
  public void OnClick()
  {
    SceneManager.LoadScene("VoteResult");
  }
}
