using System;
using System.Collections.Generic;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Controllers;

/// <summary>
/// Контроллер тусовщика карт
/// </summary>
public static class ShuffleCardController
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Потусовать карты
    /// </summary>
    /// <returns>Перечисление карт</returns>
    public static IEnumerable<CardData> ShuffleCards()
    {
        List<CardData> cardDates = new List<CardData>(GetNewDeck());
        Queue<CardData> pack = new Queue<CardData>();
        
        var count = cardDates.Count;
        
        for(var i = 0 ; i < count; i++)
            pack.Enqueue(RandomlyPlaceCard(cardDates));
        return pack;
    }
    
    private static CardData RandomlyPlaceCard(IList<CardData> cardDates)
    {
        var index = _random.Next(0, cardDates.Count - 1);
        var removeCard= cardDates[index];
        cardDates.RemoveAt(index);
        return removeCard;
    }
    
    private static IEnumerable<CardData> GetNewDeck()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            foreach (Seniority seniority in Enum.GetValues(typeof(Seniority)))
                yield return new CardData(suit, seniority);
    }
}