using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public TMP_InputField ipInput;
    public TMP_Text statusText;

    public void CriarPartida()
    {
        statusText.text = "Status: Criando partida...";
        SceneManager.LoadScene("GameScene");
    }

    public void EntrarPartida()
    {
        statusText.text = "Status: Entrando...";
        SceneManager.LoadScene("GameScene");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("MainMenu");
    }
}