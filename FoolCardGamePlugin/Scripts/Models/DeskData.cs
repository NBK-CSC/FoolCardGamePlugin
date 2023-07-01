using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные стола
/// </summary>
public struct DeskData : IDarkRiftSerializable
{
    public CardData[] UpperCards;
    public CardData[] LowerCards;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="upperCards">Верхние карты</param>
    /// <param name="lowerCards">Нижние карты</param>
    public DeskData(CardData[] upperCards, CardData[] lowerCards)
    {
        UpperCards = upperCards;
        LowerCards = lowerCards;
    }

    public void Deserialize(DeserializeEvent e)
    {
        UpperCards = e.Reader.ReadSerializables<CardData>();
        LowerCards = e.Reader.ReadSerializables<CardData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(UpperCards);
        e.Writer.Write(LowerCards);
    }
}