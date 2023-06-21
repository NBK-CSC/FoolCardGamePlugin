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

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="suit">Масть карты</param>
    /// <param name="seniority">Старшинство</param>
    public CardData(Suit suit, Seniority seniority)
    {
        Suit = suit;
        Seniority = seniority;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Suit = (Suit)e.Reader.ReadByte();
        Seniority = (Seniority)e.Reader.ReadByte();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write((byte)Suit);
        e.Writer.Write((byte)Seniority);
    }
}