using UnityEngine;
using OneSignalSDK;

public class OneSignalManager : MonoBehaviour
{
  void Start()
  {
    OneSignal.Default.Initialize("YOUR_ONESIGNAL_APP_ID");
  }
}
