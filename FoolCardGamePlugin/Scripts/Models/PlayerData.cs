using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные игрока
/// </summary>
public struct PlayerData : IDarkRiftSerializable
{
    public string RoomId { get; private set; }
    public ClientData Data;
    public byte NumberCard;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomId">Id комнанты</param>
    /// <param name="data">Данные клиента</param>
    /// <param name="numberCard">Количество карт у игрока</param>
    public PlayerData(string roomId, ClientData data, byte numberCard)
    {
        RoomId = roomId;
        Data = data;
        NumberCard = numberCard;
    }

    public void Deserialize(DeserializeEvent e)
    {
        RoomId = e.Reader.ReadString();
        Data = e.Reader.ReadSerializable<ClientData>();
        NumberCard = e.Reader.ReadByte();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(RoomId);
        e.Writer.Write(Data);
        e.Writer.Write(NumberCard);
    }
}