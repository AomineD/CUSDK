<h1>CUSDK</h1>  
Common Uses SDK  
  
With Common Uses SDK, you can make easily most common programming algorithms in Unity Games.  
  
<h3>Network Manager</h3>  
With Network manager you can easily make HTTP POST and GET request, and receive JSON serialized object.  
  
<i>(Network Manager is abstract class)</i>   

You can get JsonOBJECT and JsonArray.  
  
Example code for list (C#):  
``` C#
// this code is into a Monobehaviour class that extends NetworkManager class 

 RequestData data = new RequestData(url, RequestType.GET); // set URL and HTTP Request Type

        data.setRequestParams("name", "Javier", "lastname", "Norman"); // You can sent HTTP Request params in GET and POST
        data.setRootName("users"); // in case that you root JSONObject is not the main list


        setRequestData(data); // always set request data before load
        


        load<Usuario>(
            (response) => manageListResponse(response)
        ); // load methods always required a Serializable class like Usuario
  
```  
Example code for result request:  
``` C#
//this function receive request response.
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
        }
    }
``` 
Full example code: [MessageCloud.cs](/Assets/CUSDK/MessageCloud.cs)  
  
