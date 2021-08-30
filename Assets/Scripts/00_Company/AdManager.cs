using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
  void Start()
  {
    MobileAds.Initialize(initStatus => { });
    // DEBUG
    // List<string> deviceIds = new List<string>();
    // deviceIds.Add("2b1a8b41e2a6c6adc7dc4fb76e00d13e");
    // RequestConfiguration requestConfiguration = new RequestConfiguration
    //     .Builder()
    //     .SetTestDeviceIds(deviceIds)
    //     .build();
    // MobileAds.SetRequestConfiguration(requestConfiguration);
  }
}
