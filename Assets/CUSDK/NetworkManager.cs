using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static NetworkUtility;
using static System.Collections.Specialized.BitVector32;

public abstract class NetworkManager : MonoBehaviour
{
    private RequestData requestData;
    protected void setRequestData(RequestData data)
    {
        requestData = data;
    }

    protected void load<T>(UnityAction<Response<T>> listener)
    {
        if (requestData != null) {
            if (requestData.getType() == RequestType.GET)
                StartCoroutine(httpRequest<T>(listener));
            else
                StartCoroutine(httpRequestPost<T>(listener));
        }
        else
        {
            listener.Invoke(Response<T>.getNetworkError("No Request Data, please use setRequestData() before load"));
        }

    }

    protected void load<T>(UnityAction<ListResponse<T>> listener)
    {
        if (requestData != null)
        {
            if (requestData.getType() == RequestType.GET)
                StartCoroutine(httpRequest<T>(listener));
            else
                StartCoroutine(httpRequestPost<T>(listener));
        }
        else
        {
            listener.Invoke(ListResponse<T>.getNetworkError("No Request Data, please use setRequestData() before load"));
        }

    }

    protected void loadImage(UnityAction<Response<Sprite>> listener)
    {
        if(requestData != null)
        {
            StartCoroutine(httpRequestImage(listener));
        }
        else
        {
            listener.Invoke(Response<Sprite>.getNetworkError("No Request Data, please use setRequestData() before load"));
        }
    }

    // SINGLE IMAGE RESPONSE HTTP
    IEnumerator httpRequestImage(UnityAction<Response<Sprite>> action)
    {

        string requestUrl = getUrlFormatted();
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(requestUrl))
        {
            // null;
            UnityWebRequestAsyncOperation webRequest = request.SendWebRequest();
      

            while (!webRequest.isDone)
            {
                yield return null;
                if (requestData.getListener() != null)
                {
                    requestData.getListener().Invoke(webRequest.progress);
                }
            }

            // CHECK FOR ERROR
            if (isError(request))
            {
                requestData = null;
                action.Invoke(Response<Sprite>.getNetworkError(request.error));
            }
            else
            {
                try
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(request);
                    Sprite finalSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    Response<Sprite> response = new Response<Sprite>(finalSprite);
                    requestData = null;
                    action.Invoke(response);
                }
                catch (Exception e)
                {
                    requestData = null;
                    action.Invoke(Response<Sprite>.getNetworkError(e.Message));
           
                }


            }

        }
    
    }


    // SINGLE RESPONSE HTTP
    IEnumerator httpRequest<T>(UnityAction<Response<T>> action)
    {

        string requestUrl = getUrlFormatted();
       // Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return null;
            var webRequest = request.SendWebRequest();

            while (!webRequest.isDone)
            {
                if (requestData.getListener() != null)
                {
                    requestData.getListener().Invoke(webRequest.progress);
                }
            }

            // CHECK FOR ERROR
            if (isError(request))
            {
                requestData = null;
                action.Invoke(Response<T>.getNetworkError(request.error));
            }
            else
            {
                try {
                    T obj = JsonUtility.FromJson<T>(request.downloadHandler.text);

                    Response<T> response = new Response<T>(obj);
                    requestData = null;
                    action.Invoke(response);
                }
                catch (Exception e)
                {
                    requestData = null;
                    action.Invoke(Response<T>.getNetworkError(e.Message + " for " + request.downloadHandler.text));
                }


            }

        }
        
    }

    IEnumerator httpRequestPost<T>(UnityAction<Response<T>> action)
    {

        string requestUrl = requestData.url;
        WWWForm wWWForm = getParamsOf();




        using (UnityWebRequest request = UnityWebRequest.Post(requestUrl, wWWForm))
        {
            yield return null;
            var webRequest = request.SendWebRequest();

            while (!webRequest.isDone)
            {
                if (requestData.getListener() != null)
                {
                    requestData.getListener().Invoke(webRequest.progress);
                }
            }

            // CHECK FOR ERROR
            if (isError(request))
            {
                requestData = null;
                action.Invoke(Response<T>.getNetworkError(request.error));
            }
            else
            {
                try
                {
                    T obj = JsonUtility.FromJson<T>(request.downloadHandler.text);

                    Response<T> response = new Response<T>(obj);
                    requestData = null;
                    action.Invoke(response);
                }
                catch (Exception e)
                {
                    requestData = null;
                    action.Invoke(Response<T>.getNetworkError(e.Message + " for " + request.downloadHandler.text));
                }


            }

        }
       
    }

    // LIST RESPONSE HTTP

    IEnumerator httpRequest<T>(UnityAction<ListResponse<T>> action)
    {

        string requestUrl = getUrlFormatted();
        // Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return null;
            var webRequest = request.SendWebRequest();

            while (!webRequest.isDone)
            {
                if (requestData.getListener() != null)
                {
                    requestData.getListener().Invoke(webRequest.progress);
                }
            }

            // CHECK FOR ERROR
            if (isError(request))
            {
                requestData = null;
                action.Invoke(ListResponse<T>.getNetworkError(request.error));
            }
            else
            {
                try
                {
                    List<T> result = getFrom<T>(request.downloadHandler.text, requestData.rootName);

                    ListResponse<T> response = new ListResponse<T>(result);
                    requestData = null;
                    action.Invoke(response);
                }
                catch (Exception e)
                {
                    requestData = null;
                    action.Invoke(ListResponse<T>.getNetworkError(e.Message + " for " + request.downloadHandler.text));
                }


            }

        }
    }

    IEnumerator httpRequestPost<T>(UnityAction<ListResponse<T>> action)
    {

        string requestUrl = requestData.url;
        WWWForm wWWForm = getParamsOf();




        using (UnityWebRequest request = UnityWebRequest.Post(requestUrl, wWWForm))
        {
            yield return null;
            var webRequest = request.SendWebRequest();

            while (!webRequest.isDone)
            {
                if (requestData.getListener() != null)
                {
                    requestData.getListener().Invoke(webRequest.progress);
                }
            }

            // CHECK FOR ERROR
            if (isError(request))
            {
                requestData = null;
                action.Invoke(ListResponse<T>.getNetworkError(request.error));
            }
            else
            {
                try
                {

                    List<T> result = getFrom<T>(request.downloadHandler.text, requestData.rootName);

                    ListResponse<T> response = new ListResponse<T>(result);
                    requestData = null;
                    action.Invoke(response);
                }
                catch (Exception e)
                {
                    requestData = null;
                    action.Invoke(ListResponse<T>.getNetworkError(e.Message + " for " + request.downloadHandler.text));
                }


            }

        }
      
    }


    private WWWForm getParamsOf()
    {

        WWWForm wwwForm = new WWWForm();

        if (requestData.getRequestParams().Count > 0)
        {
            foreach (KeyValuePair<string, string> kvp in requestData.getRequestParams())
            {
                wwwForm.AddField(kvp.Key, kvp.Value);
            }
        }

        return wwwForm;

    }

    private string getUrlFormatted()
    {
        string ur = requestData.url;
        if (requestData.getRequestParams().Count > 0)
        {
            ur += "?";
            foreach (KeyValuePair<string, string> kvp in requestData.getRequestParams())
            {
                ur+=kvp.Key+"=" + kvp.Value+"&";
            }
        }

        if (ur.EndsWith("&"))
        {
            ur = ur.Substring(0, ur.Length - 1);
        }

       return ur;
    }
}


