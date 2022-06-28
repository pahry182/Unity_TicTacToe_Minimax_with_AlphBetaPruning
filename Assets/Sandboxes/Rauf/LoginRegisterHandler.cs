using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;

public class LoginRegisterHandler: MonoBehaviour
{
    public InputField inputUsername;
    public InputField inputPassword;
    public Text warningMessage;

    [Serializable]
    class UserDetail
    {
        public string username;
        public string password;
    }

    public void ValidateLoginButton()
    {
        if (inputUsername.text == string.Empty || inputPassword.text == string.Empty)
        {
            WriteWarning("Username or Password should not empty!");
            return;
        }

        StartCoroutine(RequestLogin(inputUsername.text, inputPassword.text));
    }

    public void WriteWarning(string warning)
    {
        StartCoroutine(BlinkWarningMessage());
        warningMessage.text = warning;
    }

    private IEnumerator BlinkWarningMessage()
    {
        warningMessage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.256f);
        warningMessage.gameObject.SetActive(true);
    }

    [SerializeField]
    private readonly string URL = "https://cp-api-unej.herokuapp.com/account/";

    public void ValidateRegisterButton()
    {
        if (inputUsername.text == string.Empty || inputPassword.text == string.Empty)
        {
            WriteWarning("Username or Password should not empty!");
            return;
        }

        if (inputUsername.text.Length <= 4 || inputPassword.text.Length <= 4)
        {
            WriteWarning("Username or Password must more than 4 letters long!");
            return;
        }

        StartCoroutine(RequestRegister(inputUsername.text, inputPassword.text));
    }

    private IEnumerator RequestRegister(string _username, string _password)
    {
        Profiles profile = new();
        profile.username = _username;
        profile.password = _password;
        string data = JsonUtility.ToJson(profile);
        var request = new UnityWebRequest(URL + _username, UnityWebRequest.kHttpVerbPOST);
        request.SetRequestHeader("Content-Type", "application/json");
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            WriteWarning("Error!");
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            WriteWarning("Register Completed! Hello " + _username);
            StartCoroutine(LoginSuccessful(_username));            
        }
    }

    public IEnumerator RequestLogin(string _username, string _password)
    {
        UnityWebRequest request = UnityWebRequest.Get("https://cp-api-unej.herokuapp.com/account/" + _username);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            print("Error:" + request.error);
        }
        else
        {
            print("Response:" + request.downloadHandler.text);
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(request.downloadHandler.text);

            if (userDetail.username == null)
            {
                WriteWarning("Username not found!");
            }
            else if (inputPassword.text != userDetail.password)
            {
                WriteWarning("Password is incorrect!");
            }
            else
            {
                //Save Credentials to DDOL
                WriteWarning("Login Success! Hello " + _username);
                StartCoroutine(LoginSuccessful(_username));
            }
        }
    }

    private IEnumerator LoginSuccessful(string _username)
    {
        AccountHandler.Instance.sessionUsername = _username;
        yield return new WaitForSeconds(0.512f);
        SceneManager.LoadScene("MainScene");

    }

    [System.Serializable]
    public class Profiles
    {
        public string username;
        public string password;
    }
}
