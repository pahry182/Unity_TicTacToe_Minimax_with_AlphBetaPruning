using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelector : MonoBehaviour
{
    [SerializeField] string gameTittle;
    [SerializeField] string sceneName;
    [SerializeField] string gameDescription;
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

    public string GetDescription()
    {
        return gameDescription;
    }

    public Sprite GetScreenshot()
    {
        return gameScreenshot;
    }

    public Sprite GetScreenshot2()
    {
        return gameScreenshot2;
    }
}
