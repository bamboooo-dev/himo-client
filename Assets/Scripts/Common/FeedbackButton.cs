using UnityEngine;

public class FeedbackButton : MonoBehaviour
{
  [SerializeField] private FeedbackDialog dialog = default;

  public void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    ShowDialog();
  }

  private void ShowDialog()
  {
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(GameObject.Find("Canvas").transform, false);
  }
}
