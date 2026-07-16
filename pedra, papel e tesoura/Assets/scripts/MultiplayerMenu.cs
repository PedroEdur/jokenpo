using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public TMP_InputField ipInput;
    public TMP_Text statusText;

    public void CriarPartida()
    {
        statusText.text = "Status: Criando servidor...";

       MultiplayerConfig.NetworkEnabled = true;
        MultiplayerConfig.IsHost = true;

        SceneManager.LoadScene("GameScene");
    }

    public void Entrar()
    {
        statusText.text = "Status: Conectando...";

        MultiplayerConfig.NetworkEnabled = true;
        MultiplayerConfig.IsHost = false;
       MultiplayerConfig.ServerIP = ipInput.text;

        SceneManager.LoadScene("GameScene");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("MainMenu");
    }
}