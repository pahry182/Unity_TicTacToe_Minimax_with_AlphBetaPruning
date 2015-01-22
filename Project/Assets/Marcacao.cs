using UnityEngine;
using System.Collections;

public class Marcacao : MonoBehaviour {

    public int x, y;

    JogoDaVelha jogo;

    void Start()
    {
        jogo = transform.parent.gameObject.GetComponent<TicTacToe>().jogo;
    }

    void OnMouseDown()
    {
        if (jogo.estado == JogoDaVelha.Estado.CONTINUAR)
        {
            if (jogo.MarcarMesa("X", x, y))
            {
                jogo.EstadoTeste();
                if (jogo.estado == JogoDaVelha.Estado.CONTINUAR)
                {
                    if(TicTacToe.Instance.Minimax)
					{
						Arvore a = jogo.GerarArvore();
						Minimax.MinimaxCalc(a);
						jogo.mesa = a.Jogo.mesa;
	                    TicTacToe.ultimoMovimento = Minimax.interacoes;
					}
					else if(TicTacToe.Instance.MinimaxAB)
					{
						Arvore a = jogo.GerarArvore();
						MinimaxAB.MinimaxABCalc(a);
						jogo.mesa = a.Jogo.mesa;
	                    TicTacToe.ultimoMovimento = MinimaxAB.interacoes;
					}

                    jogo.EstadoTeste();
                    if (jogo.estado == JogoDaVelha.Estado.VITORIA)
                    {
                        TicTacToe.vitoria = "Jogador 2 venceu.";
                    }
                    else if (jogo.estado == JogoDaVelha.Estado.EMPATE)
                    {
                        TicTacToe.vitoria = "Empate.";
                    }
                }
                else if (jogo.estado == JogoDaVelha.Estado.VITORIA)
                {
                    TicTacToe.vitoria = "Jogador 1 venceu.";
                }
                //TicTacToe.vitoria = "Empate.";
            }
        }
    }

    void Update()
    {
        if (jogo.mesa[x, y] == " ") renderer.material.color = Color.white;
        if (jogo.mesa[x, y] == "X") renderer.material.color = Color.red;
        if (jogo.mesa[x, y] == "O") renderer.material.color = Color.blue;
    }
}
