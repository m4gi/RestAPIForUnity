# Welcome to Basic Rest API For Unity!

**RESTClient** for Unity is built on top of [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) and Unity's [JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) to make it easier to compose REST requests and return the results serialized as native C# data model objects.
 More Function:
 - [Newtonsoft.Json](https://www.newtonsoft.com/json/help/html/introduction.htm)
 - [UniTask](https://github.com/Cysharp/UniTask)

## Features
 - Methods to add request body, headers and query strings to REST requests.
 - Ability to return result as native object or as plain text.
 - Work around for nested arrays.

## Example Usage

 1.  Add Prefab NetworkManager to scene Game wants to call API.
 2.  Fill details of API Server URL and API Path.
 3. Or use Dictionary Server to config Environment and Select Mode for config server.


This snippet shows how to **POST** a REST request
```
using BaseHttp.Api;
using BaseHttp.Core;
using Duck.Http.Service;
```

 - Build a class that represents the object in the API implement interface IResponseData.
 - Set the Serializable Attribute for the newly created class.

```
[Serializable]
public class LoginResponse : IResponseData
{
    public string success;
    public string message;
    public DataLogin data;
}
[Serializable]
public class DataLogin
{
    public string email;
    public string refresh_token;
    public string access_token;
    public string nickname;
    public long id;
    public string session;
}
```
```
public class LoginHttp : HttpApi<LoginResponse>
{
    protected override string ApiUrl => "auth/login";

    private string email;
    private string password;

    public LoginHttp(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    protected override IHttpRequest GetHttpRequest()
    {
        JSONObject data = new JSONObject();
        data.Add("email", email);
        data.Add("password", password);
        return NetworkManager.Instance.HttpPost(ApiUrl, data);
    }
}
```

```
public class LoginTest : MonoBehaviour
{
    void Start()
    {
        LoginHttp data = new LoginHttp("example@gmail.com", "password");
        data.Send(p =>
        {
            NetworkManager.Instance.SigninResponse.accessToken = p.data.access_token;
        }, _ =>
        {
            Debug.Log(_);
        });

    }
}
```

