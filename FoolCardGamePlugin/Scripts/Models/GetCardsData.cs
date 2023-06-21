using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Даннные для получения карт
/// </summary>
public struct GetCardsData : IDarkRiftSerializable
{
    public PlayerData PlayerData;
    public byte Number;
    public CardData[] Cards;

    /// <summary>
    /// Конструктр
    /// </summary>
    /// <param name="playerData">Данные игрока</param>
    /// <param name="number">Количество</param>
    /// <param name="cards"></param>
    public GetCardsData(PlayerData playerData, byte number, CardData[] cards)
    {
        PlayerData = playerData;
        Number = number;
        Cards = cards;
    }

    public void Deserialize(DeserializeEvent e)
    {
        PlayerData = e.Reader.ReadSerializable<PlayerData>();
        Number = e.Reader.ReadByte();
        Cards = e.Reader.ReadSerializables<CardData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(PlayerData);
        e.Writer.Write(Number);
        e.Writer.Write(Cards);
    }
}