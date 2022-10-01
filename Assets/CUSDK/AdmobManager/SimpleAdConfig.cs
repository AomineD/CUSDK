using System;
using UnityEngine;


[Serializable]
public class SimpleAdConfig
{
    [Space(10, order = 2)]
    [Header("Android")]
    public string androidBanner = "";
    public string androidIntersticial = "";
    public string androidReward = "";
    [Header("iOS")]
    public string iOSBanner = "";
    public string iOSIntersticial = "";
    public string iOSReward = "";

}
