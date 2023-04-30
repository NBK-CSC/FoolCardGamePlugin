using DarkRift;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Запрос комнаты
/// </summary>
public struct RoomRequest : IDarkRiftSerializable
{
    public RoomData Data;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="data"></param>
    public RoomRequest(RoomData data)
    {
        Data = data;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Data = e.Reader.ReadSerializable<RoomData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Data);
    }
}