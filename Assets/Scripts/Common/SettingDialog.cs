using UnityEngine;

public class SettingDialog : MonoBehaviour
{
  public enum DialogResult
  {
    OK,
    Cancel,
  }

  // OKボタンが押されたとき
  public void OnOk()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }

  // Cancelボタンが押されたとき
  public void OnCancel()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }
}
