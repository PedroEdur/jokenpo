using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Turnos turnos;
    public Checarresultado checarResultado;
    public UIManager uiManager;
    public AnimationController animationController;
    public CountdownUI countdownUI;

    private int vencedorRound;
    private bool processandoRound;

    public enum GameState { Resultado, Revelacao, EsperandoJogador1, EsperandoJogador2, ProximoRound, Fim }

    public GameState EstadoAtual;

    public int pontosJogador1;
    public int pontosJogador2;

    public int roundAtual = 1;
    public int maxRound = 5;

    public enum escolhadojogador { Nenhum, Pedra, Papel, Tesoura }

    public escolhadojogador escolhaJogador1 = escolhadojogador.Nenhum;
    public escolhadojogador escolhaJogador2 = escolhadojogador.Nenhum;

    private void Awake()
    {
        if (turnos == null)
        {
            turnos = FindObjectOfType<Turnos>();
        }

        if (checarResultado == null)
        {
            checarResultado = FindObjectOfType<Checarresultado>();
        }

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (animationController == null)
        {
            animationController = FindObjectOfType<AnimationController>();
        }

        if (countdownUI == null)
        {
            countdownUI = FindObjectOfType<CountdownUI>();
        }
    }

    private void Start()
    {
        MudarEstado(GameState.EsperandoJogador1);
    }

    public void MudarEstado(GameState estadoNovo)
    {
        EstadoAtual = estadoNovo;
        AtualizarInterface();
    }

    public void EscolhaJogador1(escolhadojogador escolha)
    {
        if (EstadoAtual != GameState.EsperandoJogador1 || processandoRound)
        {
            return;
        }

        escolhaJogador1 = escolha;

        if (turnos != null)
        {
            turnos.TrocarTurno();
        }

        MudarEstado(GameState.EsperandoJogador2);
    }

    public void EscolhaJogador2(escolhadojogador escolha)
    {
        if (EstadoAtual != GameState.EsperandoJogador2 || processandoRound)
        {
            return;
        }

        escolhaJogador2 = escolha;
        StartCoroutine(RevelarResultado());
    }

    public void EscolherPedra()
    {
        Escolher(escolhadojogador.Pedra);
    }

    public void EscolherPapel()
    {
        Escolher(escolhadojogador.Papel);
    }

    public void EscolherTesoura()
    {
        Escolher(escolhadojogador.Tesoura);
    }

    public void Escolher(escolhadojogador escolha)
    {
        if (EstadoAtual == GameState.EsperandoJogador1)
        {
            EscolhaJogador1(escolha);
        }
        else if (EstadoAtual == GameState.EsperandoJogador2)
        {
            EscolhaJogador2(escolha);
        }
    }

    private IEnumerator RevelarResultado()
    {
        processandoRound = true;
        MudarEstado(GameState.Revelacao);

        if (countdownUI != null)
        {
            yield return StartCoroutine(countdownUI.StartCountdown());
        }

        if (animationController != null)
        {
            animationController.MostrarEscolhas(escolhaJogador1.ToString(), escolhaJogador2.ToString());
        }

        if (checarResultado == null)
        {
            Debug.LogError("GameManager precisa de uma referência para Checarresultado.");
            processandoRound = false;
            yield break;
        }

        vencedorRound = checarResultado.olharquemvenceu(escolhaJogador1, escolhaJogador2);

        if (vencedorRound == 1)
        {
            pontosJogador1++;
        }
        else if (vencedorRound == 2)
        {
            pontosJogador2++;
        }

        processandoRound = false;
        MudarEstado(GameState.Resultado);
        MostrarResultadoRound();
    }

    public void ProximoRound()
    {
        if (EstadoAtual != GameState.Resultado)
        {
            return;
        }

        if (roundAtual >= maxRound && vencedorRound != 0)
        {
            MudarEstado(GameState.Fim);
            MostrarResultadoFinal();
            return;
        }

        if (vencedorRound != 0)
        {
            roundAtual++;
        }

        escolhaJogador1 = escolhadojogador.Nenhum;
        escolhaJogador2 = escolhadojogador.Nenhum;
        vencedorRound = 0;

        if (turnos != null)
        {
            turnos.ReiniciarTurno();
        }

        MudarEstado(GameState.EsperandoJogador1);
    }

    private void MostrarResultadoRound()
    {
        if (uiManager == null)
        {
            return;
        }

        if (vencedorRound == 0)
        {
            uiManager.ShowResult("Empate!");
        }
        else if (vencedorRound == 1)
        {
            uiManager.ShowResult("Jogador 1 venceu o round!");
        }
        else
        {
            uiManager.ShowResult("Jogador 2 venceu o round!");
        }
    }

    private void MostrarResultadoFinal()
    {
        if (uiManager == null)
        {
            return;
        }

        if (pontosJogador1 > pontosJogador2)
        {
            uiManager.ShowResult("Jogador 1 venceu o jogo!");
        }
        else if (pontosJogador2 > pontosJogador1)
        {
            uiManager.ShowResult("Jogador 2 venceu o jogo!");
        }
        else
        {
            uiManager.ShowResult("Jogo empatado!");
        }
    }

    private void AtualizarInterface()
    {
        if (uiManager == null)
        {
            return;
        }

        uiManager.UpdateScore(pontosJogador1, pontosJogador2);

        switch (EstadoAtual)
        {
            case GameState.EsperandoJogador1:
                uiManager.HideResult();
                uiManager.SetTurnText("Aguardando Jogador 1");
                break;
            case GameState.EsperandoJogador2:
                uiManager.HideResult();
                uiManager.SetTurnText("Aguardando Jogador 2");
                break;
            case GameState.Revelacao:
                uiManager.SetTurnText("Revelando escolhas...");
                break;
            case GameState.Resultado:
                uiManager.SetTurnText("Resultado do round");
                break;
            case GameState.Fim:
                uiManager.SetTurnText("Fim de jogo");
                break;
        }
    }
}

