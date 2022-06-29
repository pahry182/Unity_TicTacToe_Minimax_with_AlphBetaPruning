using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum AIDifficulty { EASY, NORMAL, HARD }


public class AISelector : MonoBehaviour
{
    private BoardAdvanced board;

    private void Awake()
    {
        board = FindObjectOfType<BoardAdvanced>();
    }

    public void SelectDifficultyButton(int difficulty = 0)
    {
        switch (difficulty)
        {
            case 1:
                BoardAdvanced.difficulty = AIDifficulty.NORMAL;
                break;
            case 2:
                BoardAdvanced.difficulty = AIDifficulty.HARD;
                break;
            default:
                BoardAdvanced.difficulty = AIDifficulty.EASY;
                break;
        }
        if (Random.Range(0, 2) == 1)
        {
            board._markC.AIMove();
        }
    }
    
    public void BackToMenuButton()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
