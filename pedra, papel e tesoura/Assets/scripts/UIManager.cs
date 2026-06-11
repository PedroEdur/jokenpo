using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Jogo")]
    public GameManager gameManager;

    [Header("Textos")]
    public TMP_Text scoreText;
    public TMP_Text turnText;
    public TMP_Text resultText;

    [Header("Painéis")]
    public GameObject resultPanel;

    private int player1Score = 0;
    private int player2Score = 0;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void Start()
    {
        UpdateScore();
        SetTurnText("Aguardando Jogador 1");

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    // ===== PLACAR =====

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = player1Score + " x " + player2Score;
        }
    }

    public void UpdateScore(int p1, int p2)
    {
        player1Score = p1;
        player2Score = p2;

        UpdateScore();
    }

    // ===== TURNO =====

    public void SetTurnText(string text)
    {
        if (turnText != null)
        {
            turnText.text = text;
        }
    }

    // ===== RESULTADO =====

    public void ShowResult(string result)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = result;
        }
    }

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    // ===== BOTÕES =====

    public void OnRockButton()
    {
        Debug.Log("Pedra escolhida");
        EnviarEscolha(GameManager.escolhadojogador.Pedra);
    }

    public void OnPaperButton()
    {
        Debug.Log("Papel escolhido");
        EnviarEscolha(GameManager.escolhadojogador.Papel);
    }

    public void OnScissorsButton()
    {
        Debug.Log("Tesoura escolhida");
        EnviarEscolha(GameManager.escolhadojogador.Tesoura);
    }

    public void OnNextRoundButton()
    {
        if (gameManager != null)
        {
            gameManager.ProximoRound();
        }
    }

    private void EnviarEscolha(GameManager.escolhadojogador escolha)
    {
        if (gameManager == null)
        {
            Debug.LogWarning("UIManager precisa de uma referência para o GameManager.");
            return;
        }

        gameManager.Escolher(escolha);
    }

    // ===== RESULTADOS =====

    public void TestVictory()
    {
        ShowResult("Vitória!");
    }

    public void TestDefeat()
    {
        ShowResult("Derrota!");
    }

    public void TestDraw()
    {
        ShowResult("Empate!");
    }
}

