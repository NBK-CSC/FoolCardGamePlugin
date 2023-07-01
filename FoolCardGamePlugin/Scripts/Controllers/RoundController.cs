using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Abstractions.Controllers;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Controllers;

/// <summary>
/// Контроллер раунда
/// </summary>
public class RoundController : IRound
{
    private readonly Dictionary<string, CardData?> _searchDefender;
    private readonly RoomConfig _roomConfig;
    private readonly CardData _trumpCard;
    private string _thrower;
    
    public bool IsThrowerIdentified { get; private set; }

    public event Action<string> OnDefenderSearched = delegate {  };
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomConfig">Конфиг комнаты</param>
    /// <param name="trumpCard">Казырная карта</param>
    public RoundController(RoomConfig roomConfig, CardData trumpCard)
    {
        _searchDefender = new Dictionary<string, CardData?>();
        _roomConfig = roomConfig;
        _trumpCard = trumpCard;
    }
    
    public void FindFirstThrowerPlayer(string playerId, IEnumerable<CardData> cards)
    {
        if (_searchDefender.ContainsKey(playerId) || IsThrowerIdentified)
            return;

        var trumpCards = cards.Where(c => c.Suit == _trumpCard.Suit);
        if (trumpCards.Any() == false)
            _searchDefender.Add(playerId, null);
        else
            _searchDefender.Add(playerId, trumpCards.OrderBy(c => c.Seniority).FirstOrDefault());
        
        if (_searchDefender.Count != _roomConfig.MaxSlots)
            return;
        _thrower = _searchDefender.Where(c => c.Value != null)
            .OrderBy(c => c.Value.Value.Seniority).FirstOrDefault().Key;

        if (string.IsNullOrEmpty(_thrower))
            _thrower = _searchDefender.First().Key;
        
        IsThrowerIdentified = true;
        OnDefenderSearched.Invoke(_thrower);
    }
}