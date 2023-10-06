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
    public bool State;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomId">Id комнанты</param>
    /// <param name="data">Данные клиента</param>
    /// <param name="numberCard">Количество карт у игрока</param>
    /// <param name="state"></param>//TODO
    public PlayerData(string roomId, ClientData data, byte numberCard, bool state)
    {
        RoomId = roomId;
        Data = data;
        NumberCard = numberCard;
        State = state;
    }

    public void Deserialize(DeserializeEvent e)
    {
        RoomId = e.Reader.ReadString();
        Data = e.Reader.ReadSerializable<ClientData>();
        NumberCard = e.Reader.ReadByte();
        State = e.Reader.ReadBoolean();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(RoomId);
        e.Writer.Write(Data);
        e.Writer.Write(NumberCard);
        e.Writer.Write(State);
    }
}