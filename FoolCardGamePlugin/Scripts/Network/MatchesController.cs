using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Abstractions.Network;
using FoolCardGamePlugin.Controllers;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Контроллер матчей
/// </summary>
public class MatchesController
{
    private readonly Dictionary<string, MatchController> _matches;

    public IMatchController? this[string key] => _matches.ContainsKey(key) ? _matches[key] : null;

    /// <summary>
    /// Конструктор
    /// </summary>
    public MatchesController()
    {
        _matches = new Dictionary<string, MatchController>();
    }
    
    /// <summary>
    /// Попытаться создать матч
    /// </summary>
    /// <param name="roomData">Данные комнаты</param>
    /// <param name="subscribeAction">Функция подписки</param>
    /// <returns>Получилось ли создать?</returns>
    public bool TryCreateMatch(RoomData roomData, Action<string> subscribeAction)
    {
        if (_matches.ContainsKey(roomData.Config.Id))
            return false;
        
        var match = new MatchController(roomData);
        match.OnMatchUpdated += subscribeAction;
        
        _matches.Add(roomData.Config.Id, match);
        
        return true;
    }

    public void RemoveMatch(string roomId, Action<string> unsubscribeAction)
    {
        if (_matches.ContainsKey(roomId) == false)
            return;
        
        _matches[roomId].OnMatchUpdated -= unsubscribeAction;
        _matches.Remove(roomId);
    }
    
    /// <summary>
    /// Получить карты
    /// </summary>
    /// <param name="data">Данные получения карт</param>
    /// <returns>Получилось ли получить?</returns>
    public bool GetCards(ref GetCardsData data)
    {
        if (_matches.ContainsKey(data.PlayerData.RoomId) == false || 
            _matches[data.PlayerData.RoomId].GetCards(data.PlayerData, data.Number, out var cards) == false)
            return false;
        
        data.Cards = cards.ToArray();
        return true;
    }

    /// <summary>
    /// Обновить данные игрока
    /// </summary>
    /// <param name="playerData"></param>
    public void UpdatePlayerData(PlayerData playerData)
    {
        if (_matches.ContainsKey(playerData.RoomId))
            return;
        
        _matches[playerData.RoomId].UpdatePlayerData(playerData);
    }
}