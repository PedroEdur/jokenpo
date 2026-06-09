using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Textos")]
    public TMP_Text scoreText;
    public TMP_Text turnText;
    public TMP_Text resultText;

    [Header("Painéis")]
    public GameObject resultPanel;

    private int player1Score = 0;
    private int player2Score = 0;

    private void Start()
    {
        UpdateScore();
        SetTurnText("Aguardando Jogador 1");

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    // Atualiza o placar
    public void UpdateScore()
    {
        scoreText.text = player1Score + " x " + player2Score;
    }

    // Caso o pessoal da lógica queira atualizar o placar
    public void UpdateScore(int p1, int p2)
    {
        player1Score = p1;
        player2Score = p2;

        UpdateScore();
    }

    // Atualiza o texto do turno
    public void SetTurnText(string text)
    {
        turnText.text = text;
    }

    // Mostra o resultado
    public void ShowResult(string result)
    {
        resultPanel.SetActive(true);
        resultText.text = result;
    }

    // Esconde o painel de resultado
    public void HideResult()
    {
        resultPanel.SetActive(false);
    }

    // ===== BOTÕES (TESTE DA INTERFACE) =====

    public void OnRockButton()
    {
        Debug.Log("Pedra escolhida");
        SetTurnText("Pedra selecionada");
    }

    public void OnPaperButton()
    {
        Debug.Log("Papel escolhida");
        SetTurnText("Papel selecionado");
    }

    public void OnScissorsButton()
    {
        Debug.Log("Tesoura escolhida");
        SetTurnText("Tesoura selecionada");
    }
}