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
    public DealerData Dealer;
    public RoomData Room;
    public List<PlayerData> Players;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dealer">Стол</param>
    /// <param name="room">Комната</param>
    /// <param name="players">Игрока</param>
    public MatchData(DealerData dealer, RoomData room, IEnumerable<PlayerData> players)
    {
        Dealer = dealer;
        Room = room;
        Players = players.ToList();
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        Dealer = e.Reader.ReadSerializable<DealerData>();
        Room = e.Reader.ReadSerializable<RoomData>();
        Players = e.Reader.ReadSerializables<PlayerData>().ToList();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Dealer);
        e.Writer.Write(Room);
        e.Writer.Write(Players.ToArray());
    }
}