using DarkRift;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Структура комнаты
/// </summary>
public struct RoomData : IDarkRiftSerializable
{
    public string Name;
    public byte Slots;
    public byte MaxSlots;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Имя комнаты</param>
    /// <param name="slots">Количество слотов</param>
    /// <param name="maxSlots">Максимальное количество слотов</param>
    public RoomData(string name, byte slots, byte maxSlots)
    {
        Name = name;
        Slots = slots;
        MaxSlots = maxSlots;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Name = e.Reader.ReadString();
        Slots = e.Reader.ReadByte();
        MaxSlots = e.Reader.ReadByte();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Name);
        e.Writer.Write(Slots);
        e.Writer.Write(MaxSlots);
    }
}