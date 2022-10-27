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
See Code Here : [LoginTest.cs](https://github.com/m4gi/RestAPIForUnity/blob/main/Http_API/Example/LoginTest.cs)
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
# Using Singleton In Unity

For making singleton object.

## Usage
See Code Here : [BaseSingleton.cs](https://github.com/m4gi/RestAPIForUnity/blob/main/Http_API/BaseSingleton.cs)
```
//========================================================
// class BaseSingleton
//========================================================
// - for making singleton object
// - usage
//		+ declare class(derived )	
//			public class OnlyOne : AutoSingletonMono< OnlyOne >
//		+ client
//			OnlyOne.Instance.[method]
//========================================================
```
**How to Use ?**

 1. Add Using BaseHttp.Utils; in the header of code.
 2.  public class OnlyOne : AutoSingletonMono< OnlyOne >
 
**-->Example:**
```
using BaseHttp.Utils;

public class ClassName: ManualSingletonMono<ClassName>{
	...
}
```
Call Singleton: 
```
            ClassName.Instance.FuncName();
```

### Class ManualSingletonMono

Singleton for mono behavior object, only return exsited object, don't create new

```
/// <summary>
/// Singleton for mono behavior object, only return exsited object, don't create new
/// </summary>
/// <typeparam name="T"></typeparam>
public class ManualSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	private static bool _applicationIsQuitting;

	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
				return null;
			
			if (_instance == null)
			{
				Debug.LogError("Cannot find Object with type " + typeof(T));
			}

			return _instance;
		}
	}

	public static bool IsInstanceValid()
	{
		if (_applicationIsQuitting)
			return false;
		
		return (_instance != null);
	}

	//MUST OVERRIDE AWAKE AT CHILD CLASS
	public virtual void Awake()
	{
		if (_instance != null)
		{
			Debug.LogWarning("Already has intsance of " + typeof(T));
			GameObject.Destroy(this.gameObject);
			return;
		}

		if (_instance == null)
			_instance = (T) (MonoBehaviour) this;

		if (_instance == null)
		{
			Debug.LogError("Awake xong van NULL " + typeof(T));
		}
		//Debug.LogError("Awake of " + typeof(T));
	}

	protected virtual void OnDestroy()
	{
		//self destroy?
		if (_instance == this)
		{
			_instance = null;
			//Debug.LogError ("OnDestroy " + typeof(T));
		}
	}


	private void OnApplicationQuit()
	{
		_applicationIsQuitting = true;
	}
}
```



### Class AutoSingletonMono

Need to create a new GameObject to attach the singleton to.

```
/// <summary>
/// Create a new GameObject to attach the singleton
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutoSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	private static bool _applicationIsQuitting;

	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
				return null;
			
			if (_instance == null)
			{
				// Need to create a new GameObject to attach the singleton to.
				var singletonObject = new GameObject();
				_instance = singletonObject.AddComponent<T>();
				singletonObject.name = typeof(T).ToString() + " (Singleton)";

			}

			return _instance;
		}
	}

	public static bool IsInstanceValid()
	{
		return (_instance != null);
	}

	//MUST OVERRIDE AWAKE AT CHILD CLASS
	public virtual void Awake()
	{
		if (_instance != null)
		{
			Debug.LogWarning("Already has intsance of " + typeof(T));
			GameObject.Destroy(this.gameObject);
			return;
		}

		if (_instance == null)
			_instance = (T) (MonoBehaviour) this;

		if (_instance == null)
		{
			Debug.LogError("Awake xong van NULL " + typeof(T));
		}
		//Debug.LogError("Awake of " + typeof(T));
	}

	protected virtual void OnDestroy()
	{
		//self destroy?
		if (_instance == this)
		{
			_instance = null;
			//Debug.LogError ("OnDestroy " + typeof(T));
		}
	}


	private void OnApplicationQuit()
	{
		_applicationIsQuitting = true;
	}
}
```


