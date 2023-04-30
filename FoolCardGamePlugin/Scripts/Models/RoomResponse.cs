using DarkRift;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Ответ по комнате
/// </summary>
public struct RoomResponse : IDarkRiftSerializable
{
    public bool ResponseState;
    public RoomData Data;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="responseState">Состояние ответа</param>
    /// <param name="data"></param>
    public RoomResponse(bool responseState, RoomData data)
    {
        ResponseState = responseState;
        Data = data;
    }

    public void Deserialize(DeserializeEvent e)
    {
        ResponseState = e.Reader.ReadBoolean();
        Data = e.Reader.ReadSerializable<RoomData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(ResponseState);
        e.Writer.Write(Data);
    }
}