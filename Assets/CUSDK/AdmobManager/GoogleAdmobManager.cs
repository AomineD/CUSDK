using GoogleMobileAds.Api;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class GoogleAdmobManager : MonoBehaviour
{
    [SerializeField]
#if UNITY_EDITOR
    [Help("Please, configure your app Android ID and iOS ID properly before use this script\n\nCheck your GoogleMobileAdsSettings.asset in Assets/GoogleMobileAds/Resources", MessageType.Info)]
#endif
    [Tooltip("This is only for check Admob status in inspector, not will change anything")]
    private bool isAdmobInitialized;
    


    public bool AdmobInitialized()
    {
        return isInitialized;
    }
    private bool isInitialized;

    protected abstract void OnInitialize();

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private AdSettings adSettings;
    //
    protected void initialize(bool protectGameObject = true, AdSettings adSettings = null)
    {
        if(protectGameObject)
        DontDestroyOnLoad(this);
        //
        MobileAds.Initialize(initStatus => {
            isInitialized = true;
            isAdmobInitialized = true;
            OnInitialize();
        });

        this.adSettings = adSettings;
    }


    public void RequestBanner()
    {
       if(adSettings == null)
        {
            return;
        }
#if UNITY_ANDROID
        string adUnitId = adSettings.getAndroidAd(AdSettings.AdType.BANNER);
#elif UNITY_IPHONE
            string adUnitId = adSettings.getiOSAd(AdSettings.AdType.BANNER);
        #else
        string adUnitId = "unexpected_platform";
#endif
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }



    public void RequestBanner(string bannerID)
    {
        string adUnitId = bannerID;
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }


    public void destroyBanner()
    {
        if (bannerView != null)
            bannerView.Destroy();
    }

    // INTERSTICIAL
    protected bool isLoadingInterstitial = false;
    public void RequestAndShowInters(UnityAction<AdListener> listener)
    {

    if(adSettings == null)
        {
            listener.Invoke(AdListener.error(null));
            return;
        }
#if UNITY_ANDROID
        string adUnitId = adSettings.getAndroidAd(AdSettings.AdType.INTERSTITIAL);
#elif UNITY_IPHONE
            string adUnitId = adSettings.getiOSAd(AdSettings.AdType.INTERSTITIAL);
#else
        string adUnitId = "unexpected_platform";
#endif

        this.actionInterstitial = listener;
        if (interstitial != null && interstitial.IsLoaded())
        {

            interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

            // Called when the ad is closed.
            interstitial.OnAdClosed += HandleOnAdClosed;

            interstitial.OnAdLoaded += HandleOnAdLoaded;



            interstitial.Show();
            return;
        }
        else if (interstitial != null && !isLoadingInterstitial && !interstitial.IsLoaded())
        {
            interstitial.Destroy();
        }

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);


        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;

        interstitial.OnAdLoaded += HandleOnAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        isLoadingInterstitial = true;
    }


    public void RequestAndShowInters(string adUnitId, UnityAction<AdListener> listener)
    {

        this.actionInterstitial = listener;
        if (interstitial != null && interstitial.IsLoaded())
        {

            interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

            // Called when the ad is closed.
            interstitial.OnAdClosed += HandleOnAdClosed;

            interstitial.OnAdLoaded += HandleOnAdLoaded;



            interstitial.Show();
            return;
        }
        else if (interstitial != null && !isLoadingInterstitial && !interstitial.IsLoaded())
        {
            interstitial.Destroy();
        }

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);


        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;

        interstitial.OnAdLoaded += HandleOnAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        isLoadingInterstitial = true;
    }

    //REWARD

    protected bool isLoadingReward = false;
    public void RequestReward(UnityAction<AdListener> listener)
    {

#if UNITY_ANDROID
        string adUnitId = adSettings.getAndroidAd(AdSettings.AdType.REWARD);
#elif UNITY_IPHONE
            string adUnitId = adSettings.getiOSAd(AdSettings.AdType.REWARD);
#else
        string adUnitId = "unexpected_platform";
#endif

        rewardedAd = new RewardedAd(adUnitId);


        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

        actionReward = listener;
        isLoadingReward = true;
    }


    public void RequestReward(string adUnitId, UnityAction<AdListener> listener)
    {

        rewardedAd = new RewardedAd(adUnitId);


        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

        actionReward = listener;
        isLoadingReward = true;
    }

    // LISTENERS

    UnityAction<AdListener> actionReward;
    UnityAction<AdListener> actionInterstitial;

    // INTERSTITIAL =====================================================================================
    protected void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isLoadingInterstitial = false;
        if (actionInterstitial != null)
        {
            actionInterstitial.Invoke(AdListener.error(args.LoadAdError.GetCause().GetMessage()));
            actionInterstitial = null;
        }
    }

    protected void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.interstitial.Destroy();
        if (actionInterstitial != null)
        {
            actionInterstitial.Invoke(AdListener.closed());
            actionInterstitial = null;
        }
    }


    protected void HandleOnAdLoaded(object sender, EventArgs args)
    {
        isLoadingInterstitial = false;
        if (actionInterstitial != null)
        {
            interstitial.Show();
            actionInterstitial.Invoke(AdListener.loaded());
            actionInterstitial = null;
        }
    }


    /** REWARD ========================================================= **/


    protected void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        isLoadingReward = false;
        if (actionReward != null)
        {
            actionReward.Invoke(AdListener.loaded());

            rewardedAd.Show();

        }
    }

    protected void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isLoadingReward = false;
        if (actionReward != null)
        {
            actionReward.Invoke(AdListener.error(args.LoadAdError.GetMessage()));
            actionReward = null;
        }
    }


    protected void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        if (actionReward != null)
        {
            actionReward.Invoke(AdListener.error(args.AdError.GetMessage()));
            actionReward = null;
        }
    }

    protected void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (actionReward != null)
        {
            actionReward.Invoke(AdListener.closed());
            actionReward = null;
        }
    }

    protected void HandleUserEarnedReward(object sender, Reward args)
    {
        if (actionReward != null)
        {
            actionReward.Invoke(AdListener.rewarded(args));
            actionReward = null;
        }
    }
}
