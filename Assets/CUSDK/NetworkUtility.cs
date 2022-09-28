using Newtonsoft.Json.Linq;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static MessageCloud;

public class NetworkUtility
{
    public static bool isError(UnityWebRequest webRequest)
    {
        return webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.DataProcessingError;
    }

    public enum RequestType
    {
        GET,
        POST
    }

    public static Dictionary<string, string> fromArray(string[] keys, string[] values)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        if(keys.Length == values.Length)
        {
            for (int i=0; i < keys.Length;i++)
            {
                result.Add(keys[i], values[i]);
            }
        }

        return result;
    }

    public static Dictionary<string, string> fromListOriented(params string[] list)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < list.Length && i + 1 < list.Length; i+=2)
            {
                result.Add(list[i], list[i+1]);
            }
        

        return result;
    }

    public static List<T> getFrom<T>(string json, string root = "")
    {
        List<T> values = new List<T>();

        JSONNode node = JSON.Parse(json);
        JSONArray array;


        if (root != "")
            array = node[root].AsArray;
        else
            array = node.AsArray;


      //  Debug.Log("json is " + json + " text-> "+ array[0]["name"] + " count "+array.Count);
        for(int i=0; i < array.Count; i++)
        {
          //  Debug.Log(array[i]+" is? "+(string.IsNullOrEmpty(array[i])));
            T m = JsonUtility.FromJson<T>(array[i].ToString());
          
            values.Add(m);
        }

        return values;

    }

}