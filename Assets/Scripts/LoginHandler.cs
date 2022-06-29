using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class LoginHandler : MonoBehaviour
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
    

    public IEnumerator RequestLogin(string _username, string _password)
    {
        WWWForm form = new();

        form.AddField("username", _username);
        form.AddField("password", _password);

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
                WriteWarning("Login Success!");
                StartCoroutine(LoginSuccessful(_username));
            }
        }
    }

    private IEnumerator LoginSuccessful(string _username)
    {
        AccountHandler.Instance.SessionUsername = _username;
        yield return new WaitForSeconds(0.512f);
        SceneManager.LoadScene("MainScene");

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
}
