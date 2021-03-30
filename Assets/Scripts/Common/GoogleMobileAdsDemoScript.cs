using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    private InterstitialAd interstitial;
    // Start is called before the first frame update
    public void OnClick()
    {
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
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
