using System.Collections;
using UnityEngine;
using Network;

public class GameManager : MonoBehaviour
{
    public Turnos turnos;
    public Checarresultado checarResultado;
    public UIManager uiManager;
    public AnimationController animationController;
    public CountdownUI countdownUI;

    [Header("Rede")]
    public bool usarRede;
    public bool jogadorLocalEhJogador1 = true;
    public NetworkManager networkManager;
    public MessengerHandler messengerHandler;

    private int vencedorRound;
    private bool processandoRound;
    private bool escolhaLocalEnviada;
    private bool inscritoNaRede;

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

        if (networkManager == null)
        {
            networkManager = FindObjectOfType<NetworkManager>();
        }

        if (messengerHandler == null)
        {
            messengerHandler = FindObjectOfType<MessengerHandler>();
        }
    }

    private void Start()
    {
        ConfigurarRede();
        MudarEstado(EstadoInicialDoRound());
    }

    private void OnDisable()
    {
        DesinscreverEventosDeRede();
    }

    private void ConfigurarRede()
    {
        usarRede = usarRede || MultiplayerConfig.NetworkEnabled;

        if (!usarRede)
        {
            return;
        }

        jogadorLocalEhJogador1 = MultiplayerConfig.IsHost;

        if (networkManager == null)
        {
            Debug.LogError("NetworkManager não encontrado na cena!");
            return;
        }

        if (messengerHandler == null)
        {
            Debug.LogError("MessengerHandler não encontrado na cena!");
            return;
        }

        messengerHandler.SetNetworkManager(networkManager);
        messengerHandler.SetLocalPlayerId(jogadorLocalEhJogador1 ? "Jogador1" : "Jogador2");
        InscreverEventosDeRede();

        if (jogadorLocalEhJogador1)
        {
            networkManager.StartServer();
        }
        else
        {
            networkManager.ConnectToServer(MultiplayerConfig.ServerIP);
        }
    }

    private void InscreverEventosDeRede()
    {
        if (inscritoNaRede || messengerHandler == null)
        {
            return;
        }

        messengerHandler.OnChoiceReceived += ReceberEscolhaRemota;
        messengerHandler.OnPlayerDisconnected += JogadorRemotoDesconectou;
        inscritoNaRede = true;
    }

    private void DesinscreverEventosDeRede()
    {
        if (!inscritoNaRede || messengerHandler == null)
        {
            return;
        }

        messengerHandler.OnChoiceReceived -= ReceberEscolhaRemota;
        messengerHandler.OnPlayerDisconnected -= JogadorRemotoDesconectou;
        inscritoNaRede = false;
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
        if (usarRede)
        {
            EscolherEmRede(escolha);
            return;
        }

        if (EstadoAtual == GameState.EsperandoJogador1)
        {
            EscolhaJogador1(escolha);
        }
        else if (EstadoAtual == GameState.EsperandoJogador2)
        {
            EscolhaJogador2(escolha);
        }
    }

    private void EscolherEmRede(escolhadojogador escolha)
    {

        {
            Debug.Log("Entrou no EscolherEmRede");
        }
        
        
        if (processandoRound || EstadoAtual == GameState.Revelacao || EstadoAtual == GameState.Resultado || EstadoAtual == GameState.Fim)
            
            
        {
            return;
        }

        if (escolhaLocalEnviada)
        {
            return;
        }

        if (jogadorLocalEhJogador1)
        {
            escolhaJogador1 = escolha;
        }
        else
        {
            escolhaJogador2 = escolha;
        }

        escolhaLocalEnviada = true;

        if (messengerHandler != null)
        {
            messengerHandler.SendChoice(escolha.ToString());
            Debug.Log("Escolha enviada: " + escolha);
        }

        AtualizarInterface();
        TentarRevelarResultadoEmRede();
    }

    private void ReceberEscolhaRemota(string senderId, string escolhaRecebida)
    {
        if (!System.Enum.TryParse(escolhaRecebida, out escolhadojogador escolha))
        {
            Debug.LogWarning($"Escolha recebida pela rede é inválida: {escolhaRecebida}");
            return;
        }

        if (jogadorLocalEhJogador1)
        {
            escolhaJogador2 = escolha;
        }
        else
        {
            escolhaJogador1 = escolha;
        }

        AtualizarInterface();
        TentarRevelarResultadoEmRede();
    }

    private void TentarRevelarResultadoEmRede()
    {
        if (processandoRound || escolhaJogador1 == escolhadojogador.Nenhum || escolhaJogador2 == escolhadojogador.Nenhum)
        {
            return;
        }

        StartCoroutine(RevelarResultado());
    }

    private void JogadorRemotoDesconectou(string senderId)
    {
        if (uiManager != null)
        {
            uiManager.ShowResult("O outro jogador saiu da partida.");
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
            animationController.MostrarEscolhas(escolhaJogador1, escolhaJogador2);
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
        escolhaLocalEnviada = false;

        if (turnos != null)
        {
            turnos.ReiniciarTurno();
        }

        MudarEstado(EstadoInicialDoRound());
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

        if (usarRede && (EstadoAtual == GameState.EsperandoJogador1 || EstadoAtual == GameState.EsperandoJogador2))
        {
            uiManager.HideResult();

            if (escolhaLocalEnviada)
            {
                uiManager.SetTurnText("Aguardando outro jogador");
            }
            else
            {
                uiManager.SetTurnText(jogadorLocalEhJogador1 ? "Sua vez: Jogador 1" : "Sua vez: Jogador 2");
            }

            return;
        }

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

    private GameState EstadoInicialDoRound()
    {
        if (usarRede && !jogadorLocalEhJogador1)
        {
            return GameState.EsperandoJogador2;
        }

        return GameState.EsperandoJogador1;
    }
}

