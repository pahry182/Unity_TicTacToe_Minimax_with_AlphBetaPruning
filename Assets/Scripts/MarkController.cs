using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MarkController : MonoBehaviour,  IPointerDownHandler
{
    public int x, y;

    private TicTacToe match;
    private Image _img;

    [Header("Sprite Reference")]
    public Sprite playerTile, computerTile, emptyTile;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    void Start()
    {
        match = transform.parent.gameObject.GetComponent<BoardAdvanced>().match;
    }

    //void OnMouseDown()
    //{
        
    //}

    void Update()
    {
        if (match.board[x, y] == " ") _img.sprite = emptyTile;
        if (match.board[x, y] == "X") _img.sprite = computerTile;
        if (match.board[x, y] == "O") _img.sprite = playerTile;
    }

    public void HardMove()
    {
        if (BoardAdvanced.Instance.Minimax)
        {
            Tree temporalTree = match.SpawnTree();
            Minimax.MinimaxCalc(temporalTree);
            match.board = temporalTree.Match.board;
            BoardAdvanced.lastInteraction = Minimax.interaction;
        }
        else if (BoardAdvanced.Instance.MinimaxAB)
        {
            Tree a = match.SpawnTree();
            MinimaxAB.MinimaxABCalc(a);
            match.board = a.Match.board;
            BoardAdvanced.lastInteraction = MinimaxAB.interactions;
        }
    }

    public void EasyMove()
    {
        int _x, _y;

        do
        {
            _x = Random.Range(0, 3);
            _y = Random.Range(0, 3);
        } while (!match.MarkTable("X", _x, _y));
    }

    public void NormalMove()
    {
        int select = Random.Range(0, 2);
        if (select == 1)
        {
            HardMove();
        }
        else
        {
            EasyMove();
        }
    }

    public void AIMove()
    {
        switch (BoardAdvanced.difficulty)
        {
            case AIDifficulty.EASY:
                EasyMove();
                break;
            case AIDifficulty.NORMAL:
                NormalMove();
                break;
            case AIDifficulty.HARD:
                HardMove();
                break;
            default:
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (match.state == TicTacToe.State.CONTINUE)
        {
            if (match.MarkTable("O", x, y))
            {
                GameManager.Instance.PlaySfx("ButtonSFX");
                match.CheckState();
                if (match.state == TicTacToe.State.CONTINUE)
                {
                    AIMove();

                    match.CheckState();
                    if (match.state == TicTacToe.State.WIN)
                    {
                        BoardAdvanced.winner = "Computer won";
                        GameManager.Instance.PlaySfx("LoseSFX");
                    }
                    else if (match.state == TicTacToe.State.DRAW)
                    {
                        BoardAdvanced.winner = "Draw";
                        GameManager.Instance.PlaySfx("LoseSFX");
                    }
                }
                else if (match.state == TicTacToe.State.WIN)
                {
                    BoardAdvanced.winner = "Player 1 won";
                    GameManager.Instance.PlaySfx("WinSFX");
                }
                else if (match.state == TicTacToe.State.DRAW)
                {
                    BoardAdvanced.winner = "Draw";
                    GameManager.Instance.PlaySfx("LoseSFX");
                }
            }
            else
            {
                GameManager.Instance.PlaySfx("InvalidSFX");
            }
        }
    }
}
