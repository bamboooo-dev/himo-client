using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
