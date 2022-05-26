using UnityEngine;
using System.Collections;

public class MarkController : MonoBehaviour {

    public int x, y;

    TicTacToe match;

    void Start()
    {
        match = transform.parent.gameObject.GetComponent<Board>().match;
    }

    void OnMouseDown()
    {
        if (match.state == TicTacToe.State.CONTINUE)
        {
            if (match.MarkTable("X", x, y))
            {
                match.CheckState();
                if (match.state == TicTacToe.State.CONTINUE)
                {
                    if(Board.Instance.Minimax)
					{
						Tree a = match.SpawnTree();
						Minimax.MinimaxCalc(a);
						match.board = a.Match.board;
	                    Board.lastInteraction = Minimax.interaction;
					}
					else if(Board.Instance.MinimaxAB)
					{
						Tree a = match.SpawnTree();
						MinimaxAB.MinimaxABCalc(a);
						match.board = a.Match.board;
	                    Board.lastInteraction = MinimaxAB.interactions;
					}

                    match.CheckState();
                    if (match.state == TicTacToe.State.WIN)
                    {
                        Board.winner = "Player 2 won";
                    }
                    else if (match.state == TicTacToe.State.DRAW)
                    {
                        Board.winner = "Draw";
                    }
                }
                else if (match.state == TicTacToe.State.WIN)
                {
                    Board.winner = "Player 1 won";
                }
            }
        }
    }

    void Update()
    {
        if (match.board[x, y] == " ") GetComponent<Renderer>().material.color = Color.white;
        if (match.board[x, y] == "X") GetComponent<Renderer>().material.color = Color.red;
        if (match.board[x, y] == "O") GetComponent<Renderer>().material.color = Color.blue;
    }
}
