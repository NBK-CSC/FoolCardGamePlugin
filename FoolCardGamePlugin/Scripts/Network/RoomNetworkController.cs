using System.Linq;
using DarkRift.Server;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Сетевой контролер комнаты
/// </summary>
public class RoomNetworkController
{
    private readonly RoomsController _roomsController;

    /// <summary>
    /// Синглтон
    /// </summary>
    //public static RoomNetworkController Instance => _instance ??= new RoomNetworkController();
    public RoomNetworkController()
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
            NetworkSender.Instance.SendRequest(Tags.CreateRoom, client.Client, new RoomConfig());
            return;
        }
        
        string id = GetNextId();
        
        _roomsController.CreateRoom(id, NetworkReader.Instance.Read<RoomConfig>(e));
        NetworkSender.Instance.SendRequest(Tags.CreateRoom, client.Client, _roomsController.Rooms[id].GetData().Config);
    }

    /// <summary>
    /// Присоединиться к комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    /// <param name="e">Сообщение</param>
    public void JoinRoom(ConnectedClient client, MessageReceivedEventArgs e)
    {
        var room = NetworkReader.Instance.Read<RoomConfig>(e);
        if (client.IsInRoom || _roomsController.JoinRoom(client.Data, room.Id) == false)
        {
            NetworkSender.Instance.SendRequest(Tags.JoinRoom, client.Client, new RoomData());
            return;
        }

        var roomData = _roomsController.Rooms[room.Id].GetData();
        
        client.IsInRoom = true;
        NetworkSender.Instance.SendRequest(Tags.JoinRoom, client.Client, roomData);

        Update(roomData);
    }

    public void UpdateInfo(MessageReceivedEventArgs e)
    {
        Update(NetworkReader.Instance.Read<RoomData>(e));
    }

    private void Update(RoomData roomData)
    {
        foreach (var clientData in roomData.Clients)
        {
            if (string.IsNullOrEmpty(clientData.Id) == false)
                NetworkSender.Instance.SendRequest(Tags.UpdateRoom, ServerManager.Instance.Clients[clientData.Id].Client, roomData);
        }
    }
    
    /// <summary>
    /// Присоединиться к комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    public void GetRooms(ConnectedClient client)
    {
        NetworkSender.Instance.SendRequest(Tags.GetRooms, client.Client, _roomsController.RoomsConfigs.ToArray());
    }

    /// <summary>
    /// Покинуть комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    public void LeaveRoom(ConnectedClient client)
    {
        if (client.IsInRoom && _roomsController.LeaveRoom(client.Data, out var roomId))
        {
            client.IsInRoom = false;
            if (string.IsNullOrEmpty(roomId) == false)
                Update(_roomsController.Rooms[roomId].GetData());
        }
    }

    private string GetNextId()
    {
        //TODO сделать генерацию
        return (_roomsController.Rooms.Values.Count() + 1).ToString();
    }
}