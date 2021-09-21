using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToVoteButton : MonoBehaviour
{
  public void OnClick()
  {
    SceneManager.LoadScene("Vote");
  }
}
