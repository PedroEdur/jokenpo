using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public TMP_InputField ipInput;
    public TMP_Text statusText;

    public void CriarPartida()
    {
        // Configura a sessão como Host
        NTsession.NetworkEnabled = true;
        NTsession.IsHost = true;

        // Não precisa informar IP para o host
        NTsession.ServerIP = "127.0.0.1";

        statusText.text = "Status: Criando partida...";

        SceneManager.LoadScene("GameScene");
    }

    public void EntrarPartida()
    {
        // Verifica se o usuário digitou um IP
        if (string.IsNullOrWhiteSpace(ipInput.text))
        {
            statusText.text = "Digite o IP do servidor.";
            return;
        }

        // Configura a sessão como Cliente
        NTsession.NetworkEnabled = true;
        NTsession.IsHost = false;

        // Salva o IP informado
        NTsession.ServerIP = ipInput.text.Trim();

        statusText.text = $"Conectando em {NTsession.ServerIP}...";

        SceneManager.LoadScene("GameScene");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("MainMenu");
    }
}