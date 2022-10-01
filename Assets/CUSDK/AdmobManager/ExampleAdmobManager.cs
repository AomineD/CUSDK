using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // SETT FROM Simple config
        AdSettings adSettings = AdSettings.simple(adConfig);
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
        RequestAndShowInters((adListener) => StateListener(adListener));
    }

    private void StateListener(AdListener adListener)
    {
        Debug.Log(adListener.ToString());
    }
}
