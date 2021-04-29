using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
  void Start()
  {
    // DEBUG
    // List<string> deviceIds = new List<string>();
    // deviceIds.Add("0ABC9F1D010941C8B8DA2A9C9A692BE8");
    // RequestConfiguration requestConfiguration = new RequestConfiguration
    //     .Builder()
    //     .SetTestDeviceIds(deviceIds)
    //     .build();
    // MobileAds.SetRequestConfiguration(requestConfiguration);
    MobileAds.Initialize(initStatus => { });
  }
}
