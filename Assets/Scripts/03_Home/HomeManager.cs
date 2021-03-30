using UnityEngine;
using GoogleMobileAds.Api;

public class HomeManager : MonoBehaviour
{
  private InterstitialAd interstitial;
  void Start()
  {
    if (RoomStatus.finished)
    {
      MobileAds.Initialize(initStatus => { });
      RequestInterstitial();
      if (this.interstitial.IsLoaded())
      {
        this.interstitial.Show();
      }
      RoomStatus.finished = false;
    }
  }


  private void RequestInterstitial()
  {

    string adUnitId = "unexpected_platform";


    // Initialize an InterstitialAd.
    this.interstitial = new InterstitialAd(adUnitId);
    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();
    // Load the interstitial with the request.
    this.interstitial.LoadAd(request);
  }

}

