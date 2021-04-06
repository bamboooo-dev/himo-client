using UnityEngine;

public class CheckButton : MonoBehaviour
{
  // ダイアログを追加する親のCanvas
  [SerializeField] private GameObject parent = default;
  // 表示するダイアログ
  [SerializeField] private ExpectDialogContainer dialogContainer;

  public void ShowDialog()
  {
    dialogContainer.gameObject.SetActive(true);
  }
}
