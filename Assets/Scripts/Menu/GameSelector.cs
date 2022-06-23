using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelector : MonoBehaviour
{
    [SerializeField] string gameTittle;
    [SerializeField] string sceneName;
    [SerializeField] Sprite gameScreenshot;
    [SerializeField] Sprite gameScreenshot2;
    
    
    public string GetTittle()
    {
        return gameTittle;
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public Sprite GetScreemshot()
    {
        return gameScreenshot;
    }
}
