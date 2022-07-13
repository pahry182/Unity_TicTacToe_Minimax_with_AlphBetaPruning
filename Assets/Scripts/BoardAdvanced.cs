using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardAdvanced : MonoBehaviour {

    public TicTacToe match;
	public bool Minimax, MinimaxAB, Negamax, NegamaxAB;
	public static int lastInteraction;
	public static string winner = "";
    public static AIDifficulty difficulty = AIDifficulty.EASY;
    public GameObject resultPanel, difficultyPanel;
    public Text resultText;

    public MarkController _markC;
	
	#region Singleton Design Pattern
    private static BoardAdvanced instance = null;
    public static BoardAdvanced Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (BoardAdvanced)FindObjectOfType(typeof(BoardAdvanced));
                if (instance == null)
                    instance = (new GameObject("TicTacToe")).AddComponent<BoardAdvanced>();
            }
            return instance;
        }
    }
    #endregion
	
    void Awake()
    {
        match = new TicTacToe();
        MinimaxAB = true;
        _markC = GetComponentInChildren<MarkController>();
    }

    private void Start()
    {
        GameManager.Instance.PlayBgm("BGM");
    }

    void OnGUI()
	{
		//if(GUI.Button(new Rect(10, 10, 100, 30), "Restart"))
		//{
		//	match.Restart();
		//	lastInteraction = 0;
		//	winner = "";
		//}
		
		//if(lastInteraction > 0)
		//{
		//	GUI.Label(new Rect(120, 10, 200, 30), lastInteraction + " interactions.");
		//}

		if(winner != "")
		{
            WinGame();

        }
		//Minimax = GUI.Toggle(new Rect(10, 50, 100, 20), !(MinimaxAB || Negamax || NegamaxAB), " Minimax");
        //MinimaxAB = GUI.Toggle(new Rect(10, 70, 300, 20), !(Minimax || Negamax || NegamaxAB), " Minimax - Alpha–beta pruning");

		// Not Implemented Yet
        //Negamax = GUI.Toggle(new Rect(10, 90, 100, 20), !(MinimaxAB || Minimax || NegamaxAB), " Negamax");
        //NegamaxAB = GUI.Toggle(new Rect(10, 110, 300, 20), !(MinimaxAB || Negamax || Minimax), " Negamax - Alpha–beta pruning");
	}

    private void WinGame()
    {
        if (resultPanel.activeSelf) return;

        resultPanel.SetActive(true);
        
        if (winner == "Player 1 won") StartCoroutine(APIScoreHandler.Instance.TictactoeScorePut());

        resultText.text = winner;
    }
    
    public void RestartGame()
    {
        resultPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        match.Restart();
        lastInteraction = 0;
        winner = "";
    }
}
    
public class TicTacToe 
{ 
    public string[,] board;
    public enum State { WIN, DRAW, CONTINUE }
    public State state;
    public int nextPlayer; // 0:Player 1; 1:Player2 (computer)

    public TicTacToe() 
    { 
        board = new string[3,3]; 
        for(int i=0; i<3; i++) 
        { 
            for(int j=0; j<3; j++) 
            { 
                board[i,j] = " "; 
            } 
        } 
        state = State.CONTINUE;
    } 
	
	public void Restart()
	{
		for(int i=0; i<3; i++) 
        { 
            for(int j=0; j<3; j++) 
            { 
                board[i,j] = " "; 
            } 
        } 
        state = State.CONTINUE;
	}
	
    public bool MarkTable(string mark, int x, int y) 
    { 
        if( x >= 3 || y >= 3 || x < 0 || y < 0) 
        {
            Debug.Log("Invalid path"); 
            return false; 
        } 
        if( board[x,y] != " ") 
        { 
            Debug.Log("Path already filled");
            return false; 
        } 
        board[x,y] = mark;
        return true; 
    } 

    public List<Vector2> PossibleMark()
    {
        List<Vector2> possibleMark = new List<Vector2>();

        for(int i=0; i<3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == " ") possibleMark.Add(new Vector2(i, j));
            }
        }

        return possibleMark;
    }
    
    public void CheckState()
    { 
	    bool isComplete = true; 
	    for(int i=0; i<3; i++) 
	    {  
		    for(int j=0; j<3; j++) 
		    { 
		        if (board[i,j] == " ") isComplete = false; 
		    } 
	    }
	    if (
	        (!board[0, 0].Equals(" ") && board[0, 0].Equals(board[0, 1]) && board[0, 1].Equals(board[0, 2])) ||
	        (!board[1, 0].Equals(" ") && board[1, 0].Equals(board[1, 1]) && board[1, 1].Equals(board[1, 2])) ||
	        (!board[2, 0].Equals(" ") && board[2, 0].Equals(board[2, 1]) && board[2, 1].Equals(board[2, 2])) ||
	        (!board[0, 0].Equals(" ") && board[0, 0].Equals(board[1, 0]) && board[1, 0].Equals(board[2, 0])) ||
	        (!board[0, 1].Equals(" ") && board[0, 1].Equals(board[1, 1]) && board[1, 1].Equals(board[2, 1])) ||
	        (!board[0, 2].Equals(" ") && board[0, 2].Equals(board[1, 2]) && board[1, 2].Equals(board[2, 2])) ||
	        (!board[0, 0].Equals(" ") && board[0, 0].Equals(board[1, 1]) && board[1, 1].Equals(board[2, 2])) ||
	        (!board[0, 2].Equals(" ") && board[0, 2].Equals(board[1, 1]) && board[1, 1].Equals(board[2, 0]))) 
	    { 
	    	state = State.WIN; 
	    }
	    else if (isComplete == true)
	    {
	        state = State.DRAW;
	    }
	    else 
        { 
        	state = State.CONTINUE; 
        } 
    } 

    public Tree SpawnTree()
    {
        if (PossibleMark().Count == 0)
        {
            return null;
        }
        else
        {
            Tree tree = new Tree(null);
            tree.Match = this;
            SpawnTree(tree);
            return tree;
        }
    }

    private static void SpawnTree(Tree tree)
    {
        if(tree.Match.state == State.CONTINUE)
        {
            List<Vector2> movements = tree.Match.PossibleMark();
            for (int i = 0; i < movements.Count; i++)
            {
                TicTacToe movement = tree.Match.Clone();
                if (movement.nextPlayer % 2 == 0) movement.MarkTable("X", (int)movements[i].x, (int)movements[i].y); 
                else movement.MarkTable("O", (int)movements[i].x, (int)movements[i].y);
				
                movement.nextPlayer++;
                movement.CheckState();

                Tree movementTree = new Tree(tree);
                movementTree.Parent = tree;
                movementTree.Match = movement;
				SpawnTree(movementTree);
            }
        }
        else
        {
            if (tree.Match.state == State.WIN)
            {
                if (tree.Match.nextPlayer % 2 == 1) tree.Value = 1;
                else tree.Value = -1;
            }
            else
            {
                tree.Value = 0;
            }
        }
    }

    public TicTacToe Clone()
    {
        TicTacToe clone = new TicTacToe();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                clone.board[i, j] = board[i, j];
            }
        }
        clone.CheckState();
        clone.nextPlayer = nextPlayer;

        return clone;
    }
}

public static class Minimax
{
    public static int interaction;
	
    public static int MinimaxCalc(Tree source)
    {
        interaction = 0;

        if (source.Leaf) return source.Value;
        else
        {
            int n = Min(source.Branches[0]);
            source.Match = source.Branches[0].Match;
            for (int i = 1; i < source.Branches.Count; i++)
            {
                int min = Min(source.Branches[i]);
                if (min > n)
                {
                    n = min;
                    source.Match = source.Branches[i].Match;
                }
            }
            return n;
        }
    }

    static int Min(Tree node)
    {
        interaction++;

        if (!node.Leaf)
        {
            node.Value = Max(node.Branches[0]);
            for (int i = 1; i < node.Branches.Count; i++)
            {
                int max = Max(node.Branches[i]);
                if (max < node.Value)
                {
                    node.Value = max;
                }
            }
        }

        return node.Value;
    }

    static int Max(Tree node)
    {
        interaction++;

        if (!node.Leaf)
        {
            node.Value = Min(node.Branches[0]);
            for (int i = 1; i < node.Branches.Count; i++)
            {
                int min = Min(node.Branches[i]);
                if (min > node.Value)
                {
                    node.Value = min;
                }
            }
        }

        return node.Value;
    }
}

public static class MinimaxAB
{
    public static int interactions;
	
    public static int MinimaxABCalc(Tree source)
    {
        interactions = 0;
		int alpha = -99;
		int beta = 99;
		
        if (source.Leaf) return source.Value;
        else
        {
            int n = Min(source.Branches[0], alpha, beta);
            source.Match = source.Branches[0].Match;
			if(n > alpha) alpha = n;
			
            for (int i = 1; i < source.Branches.Count; i++)
            {
                int min = Min(source.Branches[i], alpha, beta);
                if (min > n)
                {
                    n = min;
                    source.Match = source.Branches[i].Match;
					alpha = n;
                }
            }
            return n;
        }
    }

    static int Min(Tree node, int alpha, int beta)
    {
        interactions++;

        if (!node.Leaf)
        {
            node.Value = Max(node.Branches[0], alpha, beta);
            if(node.Value < beta) beta = node.Value;
			
            for (int i = 1; i < node.Branches.Count; i++)
            {
				if(node.Value <= alpha) break;
                int max = Max(node.Branches[i], alpha, beta);
                if (max < node.Value)
                {
                    node.Value = max;
                    beta = node.Value;
                }
            }
        }

        return node.Value;
    }

    static int Max(Tree node, int alpha, int beta)
    {
        interactions++;

        if (!node.Leaf)
        {
            node.Value = Min(node.Branches[0], alpha, beta);
            if(node.Value > alpha) alpha = node.Value;
			
            for (int i = 1; i < node.Branches.Count; i++)
            {
				if(node.Value >= beta) break;
                int min = Min(node.Branches[i], alpha, beta);
                if (min > node.Value)
                {
                    node.Value = min;
                    beta = node.Value;
                }
            }
        }

        return node.Value;
    }
}

public class Tree
{
    public Tree Parent;
    public int Value;
    public TicTacToe Match;
    public List<Tree> Branches;
    public bool Leaf
    {
        get
        {
            return (Branches.Count == 0);
        }
    }

    public Tree(Tree parent)
    {
        Parent = parent;
        Branches = new List<Tree>();

        if (Parent != null)
        {
            Parent.Branches.Add(this);
        }
    }
}