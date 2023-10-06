using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные стола
/// </summary>
public struct DealerData : IDarkRiftSerializable
{
    public byte NumberCardsInDeck { get; set; }
    public CardData TrumpCard { get; private set; }

    /// <summary>
    /// Констрктор
    /// </summary>
    /// <param name="numberCardsInDeck">Количество карт в колоде</param>
    /// <param name="trumpCard"></param>
    public DealerData(byte numberCardsInDeck, CardData trumpCard)
    {
        NumberCardsInDeck = numberCardsInDeck;
        TrumpCard = trumpCard;
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        NumberCardsInDeck = e.Reader.ReadByte();
        TrumpCard = e.Reader.ReadSerializable<CardData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(NumberCardsInDeck);
        e.Writer.Write(TrumpCard);
    }
}