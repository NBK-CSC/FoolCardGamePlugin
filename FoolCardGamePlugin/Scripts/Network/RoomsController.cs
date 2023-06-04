using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Controllers;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Котроллер комнаты
/// </summary>
public class RoomsController
{
    private readonly Dictionary<string, RoomController> _rooms;
    
    /// <summary>
    /// Словарь комнат
    /// </summary>
    public IReadOnlyDictionary<string, RoomController> Rooms => _rooms;
    
    
    public IEnumerable<RoomConfig> RoomsConfigs => _rooms.Values.Select(r => r.GetConfig());

    /// <summary>
    /// Конструктор
    /// </summary>
    public RoomsController()
    {
        _rooms = new Dictionary<string, RoomController>();
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="id">ID комнаты</param>
    /// <param name="room">Комната</param>
    public void CreateRoom(string id, RoomConfig room)
    {
        room.Id = id;
        _rooms.Add(id, new RoomController(room));
    }

    /// <summary>
    /// Удалить комнату
    /// </summary>
    /// <param name="id">ID комнаты</param>
    private void RemoveRoom(string id)
    {
        if (_rooms.ContainsKey(id))
        {
            _rooms.Remove(id);
        }
    }

    public bool JoinRoom(ClientData client, string id)
    {
        return _rooms.ContainsKey(id) && _rooms[id].TryAddClient(client);
    }

    public bool LeaveRoom(ClientData client, out string roomId)
    {
        var roomPair = _rooms.FirstOrDefault(keyValuePair => keyValuePair.Value.TryRemoveClient(client));
        if (roomPair.Equals(default))
        {
            roomId = "";
            return false;
        }
        if (roomPair.Value.IsEmpty)
            RemoveRoom(roomPair.Key);
        
        roomId = roomPair.Key;
        return true;
    }
}