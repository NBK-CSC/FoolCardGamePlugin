using System;
using System.Collections.Generic;
using System.Linq;
using DarkRift.Server;
using FoolCardGamePlugin.Abstractions.Network;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;
using FoolCardGamePlugin.Repositories;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Сетевой контролер комнаты
/// </summary>
public class RoomNetworkController
{
    private readonly RoomsController _roomsController;
    private readonly IMatchCreating _matchCreator;
    private readonly IMatchStopping _matchStopper;

    /// <summary>
    /// Синглтон
    /// </summary>
    public RoomNetworkController(IMatchCreating matchCreator, IMatchStopping matchStopper)
    {
        _matchCreator = matchCreator;
        _matchStopper = matchStopper;
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
            NetworkSender.Instance.SendResponse(Tags.CreateRoom, client.Client, new RoomConfig());
            return;
        }
        
        string id = GetNextId();
        
        _roomsController.CreateRoom(id, NetworkReader.Instance.Read<RoomConfig>(e));
        NetworkSender.Instance.SendResponse(Tags.CreateRoom, client.Client, _roomsController.Rooms[id].GetData().Config);
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
            NetworkSender.Instance.SendResponse(Tags.JoinRoom, client.Client, new RoomData());
            return;
        }

        var roomData = _roomsController.Rooms[room.Id].GetData();
        
        client.IsInRoom = true;
        NetworkSender.Instance.SendResponse(Tags.JoinRoom, client.Client, roomData);

        Update(roomData);
    }

    public void UpdateClientData(ConnectedClient client, MessageReceivedEventArgs e)
    {
        var roomData = NetworkReader.Instance.Read<RoomData>(e);
        var roomId = roomData.Config.Id;
        
        _roomsController.Rooms[roomId].UpdateClientData(roomData.Clients.Find(c => c.Id == client.Data.Id));
        
        if (CheckAllPlayerConfirm(_roomsController.Rooms[roomId].GetData().Clients) && _matchCreator.TryCreateMatch(roomData))
            _roomsController.Rooms[roomId].IsStarted = true;
        
        Update(_roomsController.Rooms[roomId].GetData());
    }

    private bool CheckAllPlayerConfirm(IEnumerable<ClientData> clients)
    {
        return clients.All(c => c.State);
    }

    private void Update(RoomData roomData)
    {
        foreach (var clientData in roomData.Clients)
        {
            if (string.IsNullOrEmpty(clientData.Id) == false)
                NetworkSender.Instance.SendResponse(Tags.UpdateRoom, ServerManager.Instance.Clients[clientData.Id].Client, roomData);
        }
    }
    
    /// <summary>
    /// Присоединиться к комнату
    /// </summary>
    /// <param name="client">Подключенный клиент</param>
    public void GetRooms(ConnectedClient client)
    {
        NetworkSender.Instance.SendResponse(Tags.GetRooms, client.Client, _roomsController.RoomsConfigs.ToArray());
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
            {
                _matchStopper.StopMatch(roomId);
                _roomsController.Rooms[roomId].IsStarted = false;
                Update(_roomsController.Rooms[roomId].GetData());
            }
        }
    }

    private string GetNextId()
    {
        //TODO сделать генерацию
        return (_roomsController.Rooms.Values.Count() + 1).ToString();
    }
}