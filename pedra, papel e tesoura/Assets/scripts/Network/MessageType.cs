namespace RockPaperScissors.Network
{
    public enum MessageType
    {
        None = 0,
        PlayerReady = 1,
        ChoiceSelected = 2,
        RoundResult = 3,
        RematchRequest = 4,
        Disconnect = 5
    }
}