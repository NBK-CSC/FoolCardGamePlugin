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
    public bool State { get; private set; }
        
    public CardData(Suit suit, Seniority seniority)
    {
        Suit = suit;
        Seniority = seniority;
        State = false;
    }
        
    public CardData()
    {
        Suit = Suit.Club;
        Seniority = Seniority.Two;
        State = true;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Suit = (Suit)e.Reader.ReadByte();
        Seniority = (Seniority)e.Reader.ReadByte();
        State = e.Reader.ReadBoolean();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write((byte)Suit);
        e.Writer.Write((byte)Seniority);
        e.Writer.Write(State);
    }
}