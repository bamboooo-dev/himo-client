using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdMobController : MonoBehaviour
{
  public BannerView bannerView;
  public void Start()
  {
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

    // Clean up banner before reusing
    if (bannerView != null)
    {
      bannerView.Destroy();
    }

    // Create a 320x50 banner at the top of the screen.
    this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();

    // Load the banner with the request.
    this.bannerView.LoadAd(request);
  }

  public void DestroyBannerAd()
  {
    if (bannerView != null)
    {
      bannerView.Destroy();
    }
  }

  private void OnDestroy()
  {
    bannerView.Destroy();
  }
}
