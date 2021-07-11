using UnityEngine;
using UnityEngine.UI;

public class QuestionDialog : MonoBehaviour
{
  public void OnCancel()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }
}
