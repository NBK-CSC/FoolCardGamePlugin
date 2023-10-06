using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные остановки раунда
/// </summary>
public struct StopRoundData : IDarkRiftSerializable
{
    public string RoomId;
    public RoundData Data;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomId">Id комнаты</param>
    /// <param name="data">Данные раунда</param>
    public StopRoundData(string roomId, RoundData data)
    {
        RoomId = roomId;
        Data = data;
    }

    public void Deserialize(DeserializeEvent e)
    {
        RoomId = e.Reader.ReadString();
        Data = e.Reader.ReadSerializable<RoundData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(RoomId);
        e.Writer.Write(Data);
    }
}