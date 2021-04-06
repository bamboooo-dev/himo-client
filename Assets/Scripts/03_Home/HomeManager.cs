using System;
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
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3882323268333157/3017982134";
    #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3882323268333157/5774151767";
    #else
        string adUnitId = "unexpected_platform";
    #endif

    // Initialize an InterstitialAd.
    this.interstitial = new InterstitialAd(adUnitId);
    this.interstitial.OnAdLoaded += HandleInterstitialBasedVideoLoaded;

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();
    // Load the interstitial with the request.
    this.interstitial.LoadAd(request);
  }

  public void HandleInterstitialBasedVideoLoaded(object sender, EventArgs args)
   {
      interstitial.Show();
   }

}

