using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerCode : MonoBehaviour
{
  private BannerView bannerView;

  public void Start()
  {
    // Initialize the Google Mobile Ads SDK.
    MobileAds.Initialize(initStatus => { });

    this.RequestBanner();
  }

  private void RequestBanner()
  {
    string adUnitId = "unexpected_platform";

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
  }
}
