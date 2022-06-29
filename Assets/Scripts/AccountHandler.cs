using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountHandler : MonoBehaviour
{
    public static AccountHandler Instance { get; private set; }
    public string SessionUsername { set; get; }

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
        SessionUsername = "";
        //back to Login Page
    }
}
