using System;
using UnityEngine;

public class AdSettings
{
    public enum AdType
    {
        BANNER,
        INTERSTITIAL,
        REWARD
    }
    private string androidBanner;
    private string androidIntersticial;
    private string androidReward;


    private string iOSBanner;
    private string iOSIntersticial;
    private string iOSReward;



    public static AdSettings simple(SimpleAdConfig adConfig)
    {
        AdSettings adSettings = new AdSettings();
        adSettings.configureiOS(adConfig.iOSBanner, adConfig.iOSIntersticial, adConfig.iOSReward);
        adSettings.configureAndroid(adConfig.androidBanner, adConfig.androidIntersticial, adConfig.androidReward);
        return adSettings;
    }


    public void configureTest()
    {
        androidBanner = "ca-app-pub-3940256099942544/6300978111";
        androidIntersticial = "ca-app-pub-3940256099942544/1033173712";
        androidReward = "ca-app-pub-3940256099942544/5224354917";

        iOSBanner = "ca-app-pub-3940256099942544/2934735716";
        iOSIntersticial = "ca-app-pub-3940256099942544/4411468910";
        iOSReward = "ca-app-pub-3940256099942544/1712485313";
    }

    public void configureAndroid(string bannerAd = "", string interstitialAd = "", string rewardAd = "")
    {
        androidBanner = bannerAd;
        androidIntersticial = interstitialAd;
        androidReward = rewardAd;
    }

    public void configureiOS(string bannerAd = "", string interstitialAd = "", string rewardAd = "")
    {
        iOSBanner = bannerAd;
        iOSIntersticial = interstitialAd;
        iOSReward = rewardAd;
    }


    public string getAndroidAd(AdType adType)
    {
        switch (adType)
        {
            case AdType.BANNER:
            return androidBanner;
            case AdType.INTERSTITIAL:
            return androidIntersticial;
            case AdType.REWARD:
            return androidReward;
        }

        return "";
    }

    public string getiOSAd(AdType adType)
    {
        switch (adType)
        {
            case AdType.BANNER:
                return iOSBanner;
            case AdType.INTERSTITIAL:
                return iOSIntersticial;
            case AdType.REWARD:
                return iOSReward;
        }

        return "";
    }

   

}
