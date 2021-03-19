using UnityEngine;

public class MiddleResultSceneManager : MonoBehaviour
{
  public VoteResult result;
  void Start()
  {
    SetResult(result);
    SetPoints(result.points);
  }
  void Update() { }

  private void SetResult(VoteResult result)
  {

  }

  private void SetPoints(int[] points)
  {

  }
}
