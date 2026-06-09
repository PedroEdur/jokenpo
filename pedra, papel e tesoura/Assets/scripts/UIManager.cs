using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text turnText;
    public TMP_Text resultText;


    public GameObject resultPanel;


    private int player1Score = 0;
    private int player2Score = 0;


    void Start()
    {
        UpdateScore();
        turnText.text = "Aguardando Jogador 1";
        resultPanel.SetActive(false);
    }


    public void UpdateScore()
    {
        scoreText.text = player1Score + " x " + player2Score;
    }


    public void ShowResult(string result)
    {
        resultPanel.SetActive(true);
        resultText.text = result;
    }


    public void HideResult()
    {
        resultPanel.SetActive(false);
    }


    public void SetTurnText(string text)
    {
        turnText.text = text;
    }
}
