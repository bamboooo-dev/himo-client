using UnityEngine;

public class CheckButton : MonoBehaviour
{
  // 表示するダイアログ
  [SerializeField] private ExpectDialogContainer dialogContainer;

  public void ShowDialog()
  {
    dialogContainer.gameObject.SetActive(true);
  }
}
