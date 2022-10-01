using GoogleMobileAds.Api;
using System;

public enum AdState
{
    Error,
    Loaded,
    Closed,
    Rewarded
}
public class AdListener
{
    private string errorMsg = "";
    private AdState adState = AdState.Error;
    private Reward rewardArgs;

    private AdListener(AdState state) { 
    adState = state;
    }

    // SIMPLE CONSTRUCTORS
    public static AdListener error(string msg)
    {
        AdListener ls = new AdListener(AdState.Error);
        ls.errorMsg = msg;
        return ls;
    }

    public static AdListener loaded()
    {
        AdListener ls = new AdListener(AdState.Loaded);
        return ls;
    }

    public static AdListener rewarded(Reward args)
    {
        AdListener ls = new AdListener(AdState.Rewarded);
        ls.rewardArgs = args;
        return ls;
    }

    public static AdListener closed()
    {
        AdListener ls = new AdListener(AdState.Closed);
        return ls;
    }

    // GETTERS
    public string getError()
    {
        return errorMsg;
    }

    public AdState getState()
    {
        return adState;
    }

    public Reward getArgs()
    {
        return rewardArgs;
    }

    public override string ToString()
    {
        return adState.ToString()+" - message - "+errorMsg.ToString();
    }
}
