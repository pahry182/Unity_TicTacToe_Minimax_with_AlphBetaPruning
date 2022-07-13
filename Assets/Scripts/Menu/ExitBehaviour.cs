using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBehaviour : MonoBehaviour
{
    public void ExitGame()
    {
        AccountHandler.Instance.LogOut();
        SceneManager.LoadScene("LoginPage");        
    }
}
