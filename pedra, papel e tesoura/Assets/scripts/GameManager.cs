using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Turnos turnos;

    private int vencedorRound;
    public Checarresultado checarResultado;

    public enum GameState {Resultado, Revelação, Esperandojogador1, Esperandojogador2, Proximoround, Fim}

    public GameState EstadoAtual;

    public int pontosJogador1;
    public int pontosJogador2;

    public int roundAtual = 1;
    public int maxRound = 5;

    public enum escolhadojogador {Nenhum, Pedra, Papel, Tesoura}

    public escolhadojogador escolhaJogador1 = escolhadojogador.Nenhum;
    public escolhadojogador escolhaJogador2 = escolhadojogador.Nenhum;

    void Start()
    {
        EstadoAtual = GameState.Esperandojogador1;
    }

    public void Mudançadeestado(GameState EstadoNovo)
    {
        EstadoAtual = EstadoNovo;
    }

   public void EscolhaJogador1(escolhadojogador escolha)
    {
        if (EstadoAtual == GameState.Esperandojogador1)
        {
            escolhaJogador1 = escolha;

            turnos.TrocarTurno();

            Mudançadeestado(GameState.Esperandojogador2);
        }
    }

    public void EscolhaJogador2(escolhadojogador escolha)
    {
        if (EstadoAtual == GameState.Esperandojogador2)
        {
            escolhaJogador2 = escolha;

            Mudançadeestado(GameState.Revelação);
        }
    }

    void Update()
    {
        switch (EstadoAtual)
        {
            case GameState.Esperandojogador1:
               
                break;
            case GameState.Esperandojogador2:
                
                break;
            case GameState.Revelação:
                int vencedor = checarResultado.olharquemvenceu(escolhaJogador1, escolhaJogador2);
                if (vencedor == 1)
                {
                    pontosJogador1++;
                }
                else if (vencedor == 2)
                {
                    pontosJogador2++;
                }
                Mudançadeestado(GameState.Resultado);
                break;
            case GameState.Resultado:

                if (roundAtual >= maxRound && vencedorRound != 0)
                {
                    Mudançadeestado(GameState.Fim);
                }
                else
                {
                    Mudançadeestado(GameState.Proximoround);
                }

            break;
            case GameState.Proximoround:

                if (vencedorRound != 0)
                {
                    roundAtual++;
                }

                escolhaJogador1 = escolhadojogador.Nenhum;
                escolhaJogador2 = escolhadojogador.Nenhum;

                turnos.ReiniciarTurno();

                Mudançadeestado(GameState.Esperandojogador1);

            break;
            case GameState.Fim:

                if (pontosJogador1 > pontosJogador2)
                {
                    
                }
                else
                {
                    
                }

            break;
        }
    }
}
