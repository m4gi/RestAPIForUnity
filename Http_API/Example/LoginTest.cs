using BaseHttp.Api;
using BaseHttp.Core;
using Duck.Http.Service;
using SimpleJSON;
using System;
using UnityEngine;

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
public class LoginTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoginHttp data = new LoginHttp("username", "password");
        data.Send(p =>
        {
            NetworkManager.Instance.SigninResponse.accessToken = p.data.access_token;
        }, _ =>
        {
            Debug.Log(_);
        });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
