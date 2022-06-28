using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountHandler : MonoBehaviour
{
    public static AccountHandler Instance { get; private set; }
    public string sessionUsername { set; get; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public void LogOut()
    {
        sessionUsername = "";
        //back to Login Page
    }
}
