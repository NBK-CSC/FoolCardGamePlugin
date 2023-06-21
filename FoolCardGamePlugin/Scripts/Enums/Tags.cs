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
        LeaveRoom = 4,
        UpdateRoom = 5,
        UpdateClient = 6,
        UpdateMatch = 50,
        UpdatePlayer = 51,
        GetCards = 52,
        StartRound = 53,
        StopRound = 54,
    }
}