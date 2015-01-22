using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour {

    public JogoDaVelha jogo;
	public bool Minimax, MinimaxAB, Negamax, NegamaxAB;
	public static int ultimoMovimento;
	public static string vitoria = "";
	
	#region Singleton Design Pattern
    private static TicTacToe instance = null;
    public static TicTacToe Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (TicTacToe)FindObjectOfType(typeof(TicTacToe));
                if (instance == null)
                    instance = (new GameObject("TicTacToe")).AddComponent<TicTacToe>();
            }
            return instance;
        }
    }
    #endregion
	
    void Awake()
    {
        jogo = new JogoDaVelha();
    }
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(10, 10, 100, 30), "Reiniciar"))
		{
			jogo.Reiniciar();
			ultimoMovimento = 0;
			vitoria = "";
		}
		
		if(ultimoMovimento > 0)
		{
			GUI.Label(new Rect(120, 10, 200, 30), ultimoMovimento + " interacoes.");
		}
		if(vitoria != "")
		{
			GUI.Box(new Rect(Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), vitoria);
		}
		Minimax = GUI.Toggle(new Rect(10, 50, 100, 20), !(MinimaxAB || Negamax || NegamaxAB), " Minimax");
        MinimaxAB = GUI.Toggle(new Rect(10, 70, 300, 20), !(Minimax || Negamax || NegamaxAB), " Minimax - Alpha–beta pruning");

		// Not Implemented Yet
        //Negamax = GUI.Toggle(new Rect(10, 90, 100, 20), !(MinimaxAB || Minimax || NegamaxAB), " Negamax");
        //NegamaxAB = GUI.Toggle(new Rect(10, 110, 300, 20), !(MinimaxAB || Negamax || Minimax), " Negamax - Alpha–beta pruning");
	}
}
    
public class JogoDaVelha 
{ 
    public string[,] mesa;
    public enum Estado { VITORIA, EMPATE, CONTINUAR }
    public Estado estado;
    public int proximoJogador; // 0:Jogador 1; 1:Jogador2 (máquina)

    public JogoDaVelha() 
    { 
        mesa = new string[3,3]; 
        for(int i=0; i<3; i++) 
        { 
            for(int j=0; j<3; j++) 
            { 
                mesa[i,j] = " "; 
            } 
        } 
        estado = Estado.CONTINUAR;
    } 
	
	public void Reiniciar()
	{
		for(int i=0; i<3; i++) 
        { 
            for(int j=0; j<3; j++) 
            { 
                mesa[i,j] = " "; 
            } 
        } 
        estado = Estado.CONTINUAR;
	}
	
    public bool MarcarMesa(string marca, int x, int y) 
    { 
        if( x >= 3 || y >= 3 || x < 0 || y < 0) 
        { 
        Debug.Log("Caminho invalido"); 
        return false; 
        } 
        if( mesa[x,y] != " ") 
        { 
        Debug.Log("Caminho ja preenchido"); 
        return false; 
        } 
        mesa[x,y] = marca; 
        return true; 
    } 

    public List<Vector2> MarcacoesPossiveis()
    {
        List<Vector2> marcacoesPossiveis = new List<Vector2>();

        for(int i=0; i<3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mesa[i, j] == " ") marcacoesPossiveis.Add(new Vector2(i, j));
            }
        }

        return marcacoesPossiveis;
    }
    
    public void EstadoTeste()
    { 
	    bool completo = true; 
	    for(int i=0; i<3; i++) 
	    {  
		    for(int j=0; j<3; j++) 
		    { 
		        if (mesa[i,j] == " ") completo = false; 
		    } 
	    }
	    if (
	        (!mesa[0, 0].Equals(" ") && mesa[0, 0].Equals(mesa[0, 1]) && mesa[0, 1].Equals(mesa[0, 2])) ||
	        (!mesa[1, 0].Equals(" ") && mesa[1, 0].Equals(mesa[1, 1]) && mesa[1, 1].Equals(mesa[1, 2])) ||
	        (!mesa[2, 0].Equals(" ") && mesa[2, 0].Equals(mesa[2, 1]) && mesa[2, 1].Equals(mesa[2, 2])) ||
	        (!mesa[0, 0].Equals(" ") && mesa[0, 0].Equals(mesa[1, 0]) && mesa[1, 0].Equals(mesa[2, 0])) ||
	        (!mesa[0, 1].Equals(" ") && mesa[0, 1].Equals(mesa[1, 1]) && mesa[1, 1].Equals(mesa[2, 1])) ||
	        (!mesa[0, 2].Equals(" ") && mesa[0, 2].Equals(mesa[1, 2]) && mesa[1, 2].Equals(mesa[2, 2])) ||
	        (!mesa[0, 0].Equals(" ") && mesa[0, 0].Equals(mesa[1, 1]) && mesa[1, 1].Equals(mesa[2, 2])) ||
	        (!mesa[0, 2].Equals(" ") && mesa[0, 2].Equals(mesa[1, 1]) && mesa[1, 1].Equals(mesa[2, 0]))) 
	    { 
	    	estado = Estado.VITORIA; 
	    }
	    else if (completo == true)
	    {
	        estado = Estado.EMPATE;
	    }
	    else 
        { 
        	estado = Estado.CONTINUAR; 
        } 
    } 

    public Arvore GerarArvore()
    {
        if (MarcacoesPossiveis().Count == 0)
        {
            return null;
        }
        else
        {
            Arvore arvore = new Arvore(null);
            arvore.Jogo = this;
            GerarArvore(arvore);
            return arvore;
        }
    }

    private static void GerarArvore(Arvore arvore)
    {
        if(arvore.Jogo.estado == Estado.CONTINUAR)
        {
            List<Vector2> movimentos = arvore.Jogo.MarcacoesPossiveis();
            for (int i = 0; i < movimentos.Count; i++)
            {
                JogoDaVelha movimento = arvore.Jogo.Clone(); // Clona o estado atual
				// Marca na posição possivel
                if (movimento.proximoJogador % 2 == 0) movimento.MarcarMesa("O", (int)movimentos[i].x, (int)movimentos[i].y); 
                else movimento.MarcarMesa("X", (int)movimentos[i].x, (int)movimentos[i].y);
				
                movimento.proximoJogador++;
                movimento.EstadoTeste();

                // -- Criando a arvore
                Arvore movimentoArvore = new Arvore(arvore);
                movimentoArvore.Pai = arvore;
                movimentoArvore.Jogo = movimento;
				GerarArvore(movimentoArvore);
            }
        }
        else
        {
            if (arvore.Jogo.estado == Estado.VITORIA)
            {
                if (arvore.Jogo.proximoJogador % 2 == 1) arvore.Valor = 1;
                else arvore.Valor = -1;
            }
            else //else if(arvore.Jogo.estado == Estado.EMPATE)
            {
                arvore.Valor = 0;
            }
        }
    }

    public JogoDaVelha Clone()
    {
        JogoDaVelha clone = new JogoDaVelha();
        //clone.mesa = mesa;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                clone.mesa[i, j] = mesa[i, j];
            }
        }
        clone.EstadoTeste();
        clone.proximoJogador = proximoJogador;

        return clone;
    }
}

public static class Minimax
{
    public static int interacoes;
	
    public static int MinimaxCalc(Arvore raiz)
    {
        interacoes = 0;

        if (raiz.Folha) return raiz.Valor;
        else
        {
            int n = Min(raiz.Ramos[0]);
            raiz.Jogo = raiz.Ramos[0].Jogo;
            for (int i = 1; i < raiz.Ramos.Count; i++)
            {
                int min = Min(raiz.Ramos[i]);
                if (min > n)
                {
                    n = min;
                    raiz.Jogo = raiz.Ramos[i].Jogo;
                }
            }
            return n;
        }
    }

    static int Min(Arvore nodo)
    {
        interacoes++;

        if (!nodo.Folha)
        {
            nodo.Valor = Max(nodo.Ramos[0]);
            //nodo.Jogo = nodo.Ramos[0].Jogo;
            for (int i = 1; i < nodo.Ramos.Count; i++)
            {
                int max = Max(nodo.Ramos[i]);
                if (max < nodo.Valor)
                {
                    nodo.Valor = max;
                    //nodo.Jogo = nodo.Ramos[i].Jogo;
                }
            }
        }

        return nodo.Valor;
    }

    static int Max(Arvore nodo)
    {
        interacoes++;

        if (!nodo.Folha)
        {
            nodo.Valor = Min(nodo.Ramos[0]);
            //nodo.Jogo = nodo.Ramos[0].Jogo;
            for (int i = 1; i < nodo.Ramos.Count; i++)
            {
                int min = Min(nodo.Ramos[i]);
                if (min > nodo.Valor)
                {
                    nodo.Valor = min;
                    //nodo.Jogo = nodo.Ramos[i].Jogo;
                }
            }
        }

        return nodo.Valor;
    }
}

public static class MinimaxAB
{
    public static int interacoes;
	
    public static int MinimaxABCalc(Arvore raiz)
    {
        interacoes = 0;
		int alpha = -99;
		int beta = 99;
		
        if (raiz.Folha) return raiz.Valor;
        else
        {
            int n = Min(raiz.Ramos[0], alpha, beta);
            raiz.Jogo = raiz.Ramos[0].Jogo;
			if(n > alpha) alpha = n;
			
            for (int i = 1; i < raiz.Ramos.Count; i++)
            {
                int min = Min(raiz.Ramos[i], alpha, beta);
                if (min > n)
                {
                    n = min;
                    raiz.Jogo = raiz.Ramos[i].Jogo;
					alpha = n;
                }
            }
            return n;
        }
    }

    static int Min(Arvore nodo, int alpha, int beta)
    {
        interacoes++;

        if (!nodo.Folha)
        {
            nodo.Valor = Max(nodo.Ramos[0], alpha, beta);
            if(nodo.Valor < beta) beta = nodo.Valor;
			
            for (int i = 1; i < nodo.Ramos.Count; i++)
            {
				if(nodo.Valor <= alpha) break;
                int max = Max(nodo.Ramos[i], alpha, beta);
                if (max < nodo.Valor)
                {
                    nodo.Valor = max;
                    beta = nodo.Valor;
                }
            }
        }

        return nodo.Valor;
    }

    static int Max(Arvore nodo, int alpha, int beta)
    {
        interacoes++;

        if (!nodo.Folha)
        {
            nodo.Valor = Min(nodo.Ramos[0], alpha, beta);
            if(nodo.Valor > alpha) alpha = nodo.Valor;
			
            for (int i = 1; i < nodo.Ramos.Count; i++)
            {
				if(nodo.Valor >= beta) break;
                int min = Min(nodo.Ramos[i], alpha, beta);
                if (min > nodo.Valor)
                {
                    nodo.Valor = min;
                    beta = nodo.Valor;
                }
            }
        }

        return nodo.Valor;
    }
}

public class Arvore
{
    public Arvore Pai;
    public int Valor;
    public JogoDaVelha Jogo;
    public List<Arvore> Ramos;
    public bool Folha
    {
        get
        {
            return (Ramos.Count == 0);
        }
    }

    public Arvore(Arvore pai)
    {
        Pai = pai;
        Ramos = new List<Arvore>();

        if (Pai != null)
        {
            Pai.Ramos.Add(this);
        }
    }
}