using System.Linq;
using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Networking.Enums;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Медиатор комнаты
/// </summary>
public class RoomMediator
{
    private static RoomMediator _instance;
    private readonly RoomController _roomController;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static RoomMediator Instance => _instance ??= new RoomMediator();
    
    private RoomMediator()
    {
        _roomController = new RoomController();
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    /// <param name="e">Сообщение</param>
    public void Create(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom)
            SendRequest(client.Client, new RoomResponse(false, new RoomData()));
        string index = GetNextId();
        _roomController.CreateRoom(index, ReadRoom(e));
        SendRequest(client.Client, new RoomResponse(true, _roomController.Rooms[index]));
    }

    private string GetNextId()
    {
        //TODO сделать генерацию
        return (_roomController.Rooms.Values.Count() + 1).ToString();
    }

    private RoomData ReadRoom(MessageReceivedEventArgs e)
    {
        using (var message = e.GetMessage())
        {
            return message.Deserialize<RoomRequest>().Data;
        }
    }

    private void SendRequest(IClient client, RoomResponse response)
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(response);
            using (var message = Message.Create((ushort)Tags.CreateRoom, writer))
            {
                client.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}