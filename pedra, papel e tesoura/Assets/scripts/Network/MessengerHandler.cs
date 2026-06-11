using System;
using RockPaperScissors.Network;
using UnityEngine;

namespace Network
{
    public class MessengerHandler : MonoBehaviour
    {
        public event Action<string> OnPlayerReady;
        public event Action<string, string> OnChoiceReceived;
        public event Action<string> OnRoundResultReceived;
        public event Action<string> OnRematchRequested;
        public event Action<string> OnPlayerDisconnected;

        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private string localPlayerId = "Player";

        private void Awake()
        {
            if (networkManager == null)
            {
                networkManager = GetComponent<NetworkManager>();
            }
        }

        private void OnEnable()
        {
            if (networkManager != null)
            {
                networkManager.OnMessageReceived += HandleRawMessage;
            }
        }

        private void OnDisable()
        {
            if (networkManager != null)
            {
                networkManager.OnMessageReceived -= HandleRawMessage;
            }
        }

        public void SendPlayerReady()
        {
            SendMessage(MessageType.PlayerReady, string.Empty);
        }

        public void SendChoice(string choice)
        {
            SendMessage(MessageType.ChoiceSelected, choice);
        }

        public void SendRoundResult(string result)
        {
            SendMessage(MessageType.RoundResult, result);
        }

        public void SendRematchRequest()
        {
            SendMessage(MessageType.RematchRequest, string.Empty);
        }

        public void SendDisconnect()
        {
            SendMessage(MessageType.Disconnect, string.Empty);
        }

        private void SendMessage(MessageType type, string payload)
        {
            NetworkMessage message = new NetworkMessage(type, localPlayerId, payload);
            string json = JsonUtility.ToJson(message);

            networkManager.Send(json);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleRawMessage(string rawMessage)
        {
            NetworkMessage message = JsonUtility.FromJson<NetworkMessage>(rawMessage);

            switch (message.Type)
            {
                case MessageType.PlayerReady:
                    OnPlayerReady?.Invoke(message.SenderId);
                    break;

                case MessageType.ChoiceSelected:
                    OnChoiceReceived?.Invoke(message.SenderId, message.Payload);
                    break;

                case MessageType.RoundResult:
                    OnRoundResultReceived?.Invoke(message.Payload);
                    break;

                case MessageType.RematchRequest:
                    OnRematchRequested?.Invoke(message.SenderId);
                    break;

                case MessageType.Disconnect:
                    OnPlayerDisconnected?.Invoke(message.SenderId);
                    break;

                default:
                    Debug.LogWarning($"Unknown message received: {rawMessage}");
                    break;
            }
        }
    }
}