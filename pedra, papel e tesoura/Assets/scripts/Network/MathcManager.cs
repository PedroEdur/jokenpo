using System;
using Network;
using UnityEngine;
using RockPaperScissors.Network;

namespace RockPaperScissors.Gameplay
{
    public class MatchManager : MonoBehaviour
    {
        public event Action<string> OnLocalChoiceSelected;
        public event Action<string> OnRemoteChoiceReceived;
        public event Action<string> OnRoundFinished;

        [SerializeField] private MessengerHandler messengerHandler;

        private string localChoice;
        private string remoteChoice;
        private bool roundFinished;

        private void Awake()
        {
            if (messengerHandler == null)
            {
                messengerHandler = FindObjectOfType<MessengerHandler>();
            }
        }

        private void OnEnable()
        {
            if (messengerHandler != null)
            {
                messengerHandler.OnChoiceReceived += HandleRemoteChoiceReceived;
            }
        }

        private void OnDisable()
        {
            if (messengerHandler != null)
            {
                messengerHandler.OnChoiceReceived -= HandleRemoteChoiceReceived;
            }
        }

        public void SelectRock()
        {
            SelectChoice("Rock");
        }

        public void SelectPaper()
        {
            SelectChoice("Paper");
        }

        public void SelectScissors()
        {
            SelectChoice("Scissors");
        }

        public void ResetRound()
        {
            localChoice = string.Empty;
            remoteChoice = string.Empty;
            roundFinished = false;
        }

        private void SelectChoice(string choice)
        {
            if (roundFinished)
            {
                return;
            }

            if (!string.IsNullOrEmpty(localChoice))
            {
                Debug.LogWarning("Local player already selected a choice for this round.");
                return;
            }

            localChoice = choice;
            OnLocalChoiceSelected?.Invoke(localChoice);

            messengerHandler.SendChoice(choice);

            TryFinishRound();
        }

        private void HandleRemoteChoiceReceived(string senderId, string choice)
        {
            if (roundFinished)
            {
                return;
            }

            remoteChoice = choice;
            OnRemoteChoiceReceived?.Invoke(remoteChoice);

            TryFinishRound();
        }

        private void TryFinishRound()
        {
            if (string.IsNullOrEmpty(localChoice) || string.IsNullOrEmpty(remoteChoice))
            {
                return;
            }

            string result = GetRoundResult(localChoice, remoteChoice);
            roundFinished = true;

            OnRoundFinished?.Invoke(result);
            messengerHandler.SendRoundResult(result);
        }

        private string GetRoundResult(string local, string remote)
        {
            if (local == remote)
            {
                return "Draw";
            }

            bool localWon =
                local == "Rock" && remote == "Scissors" ||
                local == "Paper" && remote == "Rock" ||
                local == "Scissors" && remote == "Paper";

            if (localWon)
            {
                return "Win";
            }

            return "Lose";
        }
    }
}