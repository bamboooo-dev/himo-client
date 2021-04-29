using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class HomeManager : MonoBehaviour
{
  private InterstitialAd interstitial;
  private bool show;
  void Start()
  {
    RequestInterstitial();
  }

  void OnGUI()
  {
    ShowAd();
  }

  void ShowAd()
  {
    if (RoomStatus.finished && this.interstitial.IsLoaded())
    {
      RoomStatus.finished = false;
      this.interstitial.Show();
    }
  }

  private void RequestInterstitial()
  {
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3882323268333157/3017982134";

    // DEBUG
    // string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
      string adUnitId = "ca-app-pub-3882323268333157/5774151767";
#else
      string adUnitId = "unexpected_platform";
#endif
    this.interstitial = new InterstitialAd(adUnitId);
    AdRequest request = new AdRequest.Builder().Build();
    this.interstitial.OnAdClosed += InterstitialAd_OnAdClosed;
    this.interstitial.LoadAd(request);
  }

  void InterstitialAd_OnAdClosed(object sender, System.EventArgs e)
  {
    DestroyInterstitial();
  }

  public void DestroyInterstitial()
  {
    this.interstitial.Destroy();
  }
}

