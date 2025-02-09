using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Text tittleText;
    [SerializeField] ChangeScene changeScene;
    [SerializeField] Text descriptionText;
    [SerializeField] Image screenshotImage1;
    [SerializeField] Image screenshotImage2;
    GameSelector gameSelector;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        gameSelector = GetComponent<GameSelector>();
        
    }

    private void Start() {
        tittleText.text = "Select Game";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectGame()
    {
        tittleText.text = gameSelector.GetTittle();
        descriptionText.text = gameSelector.GetDescription();
        screenshotImage1.sprite = gameSelector.GetScreenshot();
        screenshotImage2.sprite = gameSelector.GetScreenshot2();
        changeScene.SetSceneName(gameSelector.GetSceneName());
    }
}
