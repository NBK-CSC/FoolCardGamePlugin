using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Controllers;

public class RoundController
{
    private readonly Dictionary<string, CardData?> _searchDefender;
    private readonly RoomConfig _roomConfig;
    private readonly CardData _trumpCard;
    private string _defender;
    
    public bool IsDefenderIdentified { get; private set; }

    public event Action<string> OnDefenderSearched = delegate {  };
    
    public RoundController(RoomConfig roomConfig, CardData trumpCard)
    {
        _searchDefender = new Dictionary<string, CardData?>();
        _roomConfig = roomConfig;
        _trumpCard = trumpCard;
    }
    
    public void FindDefenderPlayer(string playerId, IEnumerable<CardData> cards)
    {
        if (_searchDefender.ContainsKey(playerId) || IsDefenderIdentified)
            return;

        var trumpCards = cards.Where(c => c.Suit == _trumpCard.Suit);
        if (trumpCards.Any() == false)
        {
            _searchDefender.Add(playerId, null);
            return;
        }
        
        _searchDefender.Add(playerId, trumpCards.OrderBy(c => c.Seniority).FirstOrDefault());
        
        if (_searchDefender.Count != _roomConfig.MaxSlots)
            return;
        
        _defender = _searchDefender.Where(c => c.Value != null)
            .Aggregate((l, r) => l.Value.Value.Seniority < r.Value.Value.Seniority ? l : r).Key;
        IsDefenderIdentified = true;
        OnDefenderSearched.Invoke(_defender);
    }
}