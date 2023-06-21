using System;
using System.Collections.Generic;
using System.Linq;
using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные матча
/// </summary>
public struct MatchData : IDarkRiftSerializable
{
    public DeskData Desk;
    public RoomData Room;
    public List<PlayerData> Players;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="desk">Стол</param>
    /// <param name="room">Комната</param>
    /// <param name="players">Игрока</param>
    public MatchData(DeskData desk, RoomData room, IEnumerable<PlayerData> players)
    {
        Desk = desk;
        Room = room;
        Players = players.ToList();
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        Desk = e.Reader.ReadSerializable<DeskData>();
        Room = e.Reader.ReadSerializable<RoomData>();
        Players = e.Reader.ReadSerializables<PlayerData>().ToList();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Desk);
        e.Writer.Write(Room);
        e.Writer.Write(Players.ToArray());
    }
}