using UnityEngine;

public class QuestionButton : MonoBehaviour
{
  [SerializeField] private GameObject parent = default;
  [SerializeField] private QuestionDialog dialog = default;

  public void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    ShowDialog();
  }

  private void ShowDialog()
  {
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(parent.transform, false);
  }
}
