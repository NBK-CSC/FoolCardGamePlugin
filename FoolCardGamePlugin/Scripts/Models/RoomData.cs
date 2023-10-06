using System.Collections.Generic;
using System.Linq;
using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные комнаты
/// </summary>
public struct RoomData : IDarkRiftSerializable
{
    public RoomConfig Config;
    public List<ClientData> Clients;
    public bool IsStarted;

    /// <summary>
    /// Конструктор
    /// </summary>
    public RoomData() : this(new RoomConfig()) { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="config">Конфиг комнаты</param>
    public RoomData(RoomConfig config)
    {
        Config = config;
        Clients = new List<ClientData>(Config.MaxSlots);
        IsStarted = false;
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        Config = e.Reader.ReadSerializable<RoomConfig>();
        Clients = e.Reader.ReadSerializables<ClientData>().ToList();
        IsStarted = e.Reader.ReadBoolean();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Config);
        e.Writer.Write(Clients.ToArray());
        e.Writer.Write(IsStarted);
    }
}