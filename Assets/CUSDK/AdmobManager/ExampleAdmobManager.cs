using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// You should extend your manager to GoogleAdmobManager
public class ExampleAdmobManager : GoogleAdmobManager
{
    // You can use SimpleAdConfig to set all ID from inspector
    
    public SimpleAdConfig adConfig = new SimpleAdConfig();
    protected override void OnInitialize()
    {
        // Admob was successfully initialized
        Invoke("loadAd", 2);
    }

    private void Start()
    {
        // SET FROM Simple config
        AdSettings adSettings = AdSettings.simple(adConfig);

        // OR Manual settings
        adSettings = new AdSettings();
        adSettings.configureAndroid("banner", "intersticial", "reward");
        adSettings.configureiOS("banner", "intersticial", "reward");
        // You can let empty any ID field if you won't use.

        // Configure test ads will replace your original ad units
        adSettings.configureTest();
        //You must initialize in Start() or Awake()
        initialize(
            protectGameObject: true, // dont destroy on load
            adSettings: adSettings // set GENERAL settings if you want
            );
    }

    // load intersticial test
    void loadAd()
    {
        // I RECOMMEND YOU TO USE SEPARATED LISTENERS FOR EACH AD TYPE
        UnityAction<AdListener> listener = (adListener) => StateListener(adListener);

        /** LOAD BANNERS **/

        // BANNER FROM AD SETTINGS
        RequestBanner();

        // OR SET ID MANUALLY
        RequestBanner("banner");

        // ADAPTATIVE (ONLY WITH AD SETTINGS)
        RequestBannerAdaptative();

        // YOU CAN SET POSITIONS AND SIZE
        RequestBanner(AdSize.MediumRectangle, AdPosition.Center);


        /** LOAD INTERSTITIAL **/

        // INTERSTITIAL FROM AD SETTINGS
        RequestAndShowInters(listener);

        // OR SET ID MANUALLY
        RequestAndShowInters("intersticial", (adListener) => StateListener(adListener));


        /** LOAD REWARD **/

        //Reward
        RequestReward(listener);

        // OR SET ID MANUALLY
        RequestReward("reward", listener);
    }

    private void StateListener(AdListener adListener)
    {
        Debug.Log(adListener.ToString());
    }
}
