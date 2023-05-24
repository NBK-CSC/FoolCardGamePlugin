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
        LeaveRoom = 4
    }
}