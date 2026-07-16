using System;
using UnityEngine;

public class MessengerHandler : MonoBehaviour
{
    private NetworkManager networkManager;

    private string localPlayerId = "Jogador";

    public event Action<string, string> OnChoiceReceived;
    public event Action OnNextRoundReceived;
    public event Action<int> OnResultReceived;
    public event Action OnCountdownReceived;
    public event Action<string> OnPlayerDisconnected;

    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        if (networkManager != null)
        {
            networkManager.OnMessageReceived += ProcessMessage;
        }
    }

    private void OnDestroy()
    {
        if (networkManager != null)
        {
            networkManager.OnMessageReceived -= ProcessMessage;
        }
    }

    public void SetNetworkManager(NetworkManager manager)
    {
        networkManager = manager;
    }

    public void SetLocalPlayerId(string id)
    {
        localPlayerId = id;
    }

    public void SendChoice(string escolha)
    {
        if (networkManager == null) return;

        networkManager.Send($"CHOICE|{localPlayerId}|{escolha}");
    }

    public void SendNextRound()
    {
        if (networkManager == null) return;

        networkManager.Send("NEXTROUND");
    }

    public void SendCountdown()
    {
        if (networkManager == null) return;

        networkManager.Send("COUNTDOWN");
    }

    public void SendResult(int vencedor)
    {
        if (networkManager == null) return;

        networkManager.Send($"RESULT|{vencedor}");
    }

    private void ProcessMessage(string message)
    {
        string[] dados = message.Split('|');

        switch (dados[0])
        {
            case "CHOICE":

                if (dados.Length >= 3)
                {
                    string jogador = dados[1];
                    string escolha = dados[2];

                    OnChoiceReceived?.Invoke(jogador, escolha);
                }

                break;

            case "NEXTROUND":

                OnNextRoundReceived?.Invoke();

                break;

            case "COUNTDOWN":

                OnCountdownReceived?.Invoke();

                break;

            case "RESULT":

                if (dados.Length >= 2)
                {
                    int vencedor;

                    if (int.TryParse(dados[1], out vencedor))
                    {
                        OnResultReceived?.Invoke(vencedor);
                    }
                }

                break;

            case "DISCONNECT":

                if (dados.Length >= 2)
                {
                    OnPlayerDisconnected?.Invoke(dados[1]);
                }

                break;
        }
    }

    public void Disconnect()
    {
        if (networkManager != null)
        {
            networkManager.Send($"DISCONNECT|{localPlayerId}");
        }
    }
}