using System;
using System.Collections.Generic;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Котроллер комнаты
/// </summary>
public class RoomController
{
    private readonly Dictionary<string, RoomData> _rooms;
    
    /// <summary>
    /// Словарь комнат
    /// </summary>
    public IReadOnlyDictionary<string, RoomData> Rooms => _rooms;

    /// <summary>
    /// Конструктор
    /// </summary>
    public RoomController()
    {
        _rooms = new Dictionary<string, RoomData>();
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="id">ID комнаты</param>
    /// <param name="room">Комната</param>
    public void CreateRoom(string id, RoomData room)
    {
        Console.WriteLine($"Создание комнаты id = {id}");
        _rooms.Add(id, room);
    }

    /// <summary>
    /// Удалить комнату
    /// </summary>
    /// <param name="id">ID комнаты</param>
    public void RemoveRoom(string id)
    {
        if (_rooms.ContainsKey(id))
            _rooms.Remove(id);
    }
}