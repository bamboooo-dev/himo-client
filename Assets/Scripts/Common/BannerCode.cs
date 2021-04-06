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
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3882323268333157/3741511642";
    #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3882323268333157/7655763978";
    #else
        string adUnitId = "unexpected_platform";
    #endif

    // Create a 320x50 banner at the top of the screen.
    AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

    this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
    this.bannerView.OnAdLoaded += HandleBannerBasedVideoLoaded;

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();

    // Load the banner with the request.
    this.bannerView.LoadAd(request);
  }

  public void HandleBannerBasedVideoLoaded(object sender, EventArgs args)
   {
      bannerView.Show();
   }
}
