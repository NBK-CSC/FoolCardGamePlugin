namespace FoolCardGamePlugin.Network.Enums
{
    /// <summary>
    /// Теги отправки сообщений
    /// </summary>
    public enum Tags
    {
        CreateRoom = 0,
        GetRooms = 1,
        JoinRoom = 2,
        LeaveRoom = 3,
        UpdateRoom = 4,
        UpdateClient = 5,
        GetMatch = 50,
        UpdateDesk = 51,
        UpdatePlayer = 52,
        GetCards = 53,
        StartRound = 54,
        StopRound = 55,
        ThrowCard = 56,
    }
}