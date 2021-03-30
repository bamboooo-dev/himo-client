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

    // 音量を保存する
    PlayerPrefs.SetFloat("BGMVolume", AudioManager.GetInstance().BGMVolume);
    PlayerPrefs.SetFloat("SEVolume", AudioManager.GetInstance().SEVolume);
    PlayerPrefs.Save();
    Destroy(this.gameObject);
  }

  // Cancelボタンが押されたとき
  public void OnCancel()
  {
    AudioManager.GetInstance().PlaySound(0);
    Destroy(this.gameObject);
  }
}
