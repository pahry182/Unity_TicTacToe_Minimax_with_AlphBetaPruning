using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] string sceneName;
    // Start is called before the first frame update
    
    public string SetSceneName(string name)
    {
        return sceneName = name;
    }

    public void ChangeGameScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    
}
