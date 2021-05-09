using UnityEngine;

public class AlertUpdateDialog : MonoBehaviour
{
  public void OnClickStoreButton()
  {
#if UNITY_ANDROID
    string storeURL = "https://play.google.com/store/apps/details?id=com.bamboooo.waiwai";
#elif UNITY_IPHONE
    string storeURL = "itms-apps://itunes.apple.com/app/id1561027910";
#else
    string storeURL = "itms-apps://itunes.apple.com/app/id1561027910";
#endif
    Application.OpenURL(storeURL);
  }

  public void OnCancel()
  {
    this.gameObject.SetActive(false);
  }
}
