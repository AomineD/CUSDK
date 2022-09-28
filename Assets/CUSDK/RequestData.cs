using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static NetworkUtility;

public class RequestData
{
    internal string url = "";
    private RequestType type;
    private string rootArrayName = "";
    internal string rootName {
        get { return rootArrayName; }
    }
    private Dictionary<string, string> requestParams = new Dictionary<string, string>();
    private UnityAction<float> progressListener = null;

    public RequestData(string url, RequestType type)
    {
        this.url = url;
        this.type = type;
    
    }

    /// <summary>
    /// This method set requestParams in POST or GET request, you can use inline dictionary
    /// </summary>
    public void setRequestParams(Dictionary<string, string> requestParams)
    {
        this.requestParams = requestParams;
    }

    /// <summary>
    /// This method set requestParams in POST or GET request, you can use inline key-value pars
    /// </summary>
    public void setRequestParams(params string[] list)
    {
        this.requestParams = NetworkUtility.fromListOriented(list);
    }


    /// <summary>
    /// This method set name of root array for List request
    /// </summary>
    public void setRootName(string root)
    {
        rootArrayName = root;
    }

    public void setProgressListener(UnityAction<float> progressListener)
    {
        this.progressListener = progressListener;
    }

    public UnityAction<float> getListener()
    {
        return progressListener;
    }


    public Dictionary<string, string> getRequestParams()
    {
        return requestParams;
    }


    public RequestType getType()
    {
        return type;
    }

}
