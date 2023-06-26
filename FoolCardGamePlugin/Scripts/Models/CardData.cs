using DarkRift;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные карты
/// </summary>
public struct CardData : IDarkRiftSerializable
{
    public Suit Suit { get; private set; }
    public Seniority Seniority { get; private set; }
    public bool IsEmpty { get; private set; }
        
    public CardData(Suit suit, Seniority seniority)
    {
        Suit = suit;
        Seniority = seniority;
        IsEmpty = true;
    }
        
    public CardData(bool isEmpty = false)
    {
        Suit = Suit.Club;
        Seniority = Seniority.Two;
        IsEmpty = isEmpty;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Suit = (Suit)e.Reader.ReadByte();
        Seniority = (Seniority)e.Reader.ReadByte();
        IsEmpty = e.Reader.ReadBoolean();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write((byte)Suit);
        e.Writer.Write((byte)Seniority);
        e.Writer.Write(IsEmpty);
    }
}