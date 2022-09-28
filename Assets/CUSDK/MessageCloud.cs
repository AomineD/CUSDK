using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NetworkUtility;

public class MessageCloud : NetworkManager
{
    public string urlImage = "";
    public string url = "";
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("loadf", 2f);
    }

    void loadf()
    {
        RequestData data = new RequestData(url, RequestType.GET);

        data.setRequestParams("name", "Javier", "lastname", "Norman");
      //  data.setRootName("users");


        setRequestData(data);
        


        load<Usuario>(
            (response) => manageListResponse(response)
        );


  

    }

    private void manageimg(Response<Sprite> response)
    {
        if (response.responseStatus() == ResponseStatus.ERROR)
        {
            Debug.LogError("ERROR NETWORK -> " + response.responseMessage);
        }
        else
        {
            img.sprite = response.GetValue();
        }
    }

    private void manageResponse(Response<Usuario> response)
    {
       if(response.responseStatus() == ResponseStatus.ERROR)
        {
            Debug.LogError("ERROR NETWORK -> "+response.responseMessage);
        }
        else
        {
            Debug.Log(response.GetValue());
        }
    }

    private void manageListResponse(ListResponse<Usuario> response)
    {
        if (response.responseStatus() == ResponseStatus.ERROR)
        {
            Debug.LogError("ERROR NETWORK -> " + response.responseMessage);
        }
        else
        {
            foreach(Usuario user in response.GetValues()) {
                Debug.Log(user);
            }
            loadImageNow();
        }
    }

    private void loadImageNow()
    {
        RequestData imag = new RequestData(urlImage, RequestType.GET);
        imag.setProgressListener((progress) => img.fillAmount = progress);
        setRequestData(imag);
        
        loadImage(
            (response) => manageimg(response)
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public class Usuario
    {
        public string name;
        public string lastname;
        public string age;

     public override string ToString()
        {
            return name + " - " + lastname + " - " + age;
        }
    }
}
