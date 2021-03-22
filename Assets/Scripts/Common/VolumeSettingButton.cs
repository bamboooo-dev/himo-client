using UnityEngine;

public class VolumeSettingButton : MonoBehaviour
{
  [SerializeField] private GameObject parent = default;
  [SerializeField] private SettingDialog dialog = default;
  public void OnClick()
  {
    AudioManager.GetInstance().PlaySound(0);
    ShowDialog();
  }
  public void ShowDialog()
  {
    // 生成してCanvasの子要素に設定
    var _dialog = Instantiate(dialog);
    _dialog.transform.SetParent(parent.transform, false);
  }
}
