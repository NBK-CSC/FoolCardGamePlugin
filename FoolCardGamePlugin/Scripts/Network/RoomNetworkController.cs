using System;
using System.Linq;
using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Медиатор комнаты
/// </summary>
public class RoomNetworkController
{
    private static RoomNetworkController _instance;
    private readonly RoomsController _roomsController;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static RoomNetworkController Instance => _instance ??= new RoomNetworkController();
    
    private RoomNetworkController()
    {
        _roomsController = new RoomsController();
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    /// <param name="e">Сообщение</param>
    public void Create(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom)
        {
            SendRequest(Tags.CreateRoom, client.Client, new RoomConfig());
            return;
        }
        
        string id = GetNextId();
        
        _roomsController.CreateRoom(id, ReadRoom(e));
        SendRequest(Tags.CreateRoom, client.Client, _roomsController.Rooms[id].GetData().Config);
    }

    /// <summary>
    /// Присоединиться к комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    /// <param name="e">Сообщение</param>
    public void JoinRoom(ConnectedClient client, MessageReceivedEventArgs e)
    {
        var room = ReadRoom(e);
        if (client.IsInRoom || _roomsController.JoinRoom(client.Data, room.Id) == false)
        {
            SendRequest(Tags.JoinRoom, client.Client, new RoomData());
            return;
        }

        client.IsInRoom = true;
        SendRequest(Tags.JoinRoom, client.Client, _roomsController.Rooms[room.Id].GetData());
    }

    public void LeaveRoom(ConnectedClient client)
    {
        if (client.IsInRoom && _roomsController.LeaveRoom(client.Data))
            client.IsInRoom = false;
    }

    private string GetNextId()
    {
        //TODO сделать генерацию
        return (_roomsController.Rooms.Values.Count() + 1).ToString();
    }

    private RoomConfig ReadRoom(MessageReceivedEventArgs e)
    {
        using (var message = e.GetMessage())
        {
            return message.Deserialize<RoomConfig>();
        }
    }
    
    private void SendRequest<T>(Tags tag, IClient client, T data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }

    private void SendRequest<T>(Tags tag, IClient client, T[] data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }
    
    private void SendMessage(Tags tag, IClient client, DarkRiftWriter writer, SendMode sendMode)
    {
        using (var message = Message.Create((ushort)tag, writer))
        {
            client.SendMessage(message, sendMode);
        }
    }
}