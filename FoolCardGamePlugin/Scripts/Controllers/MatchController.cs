using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Abstractions.Controllers;
using FoolCardGamePlugin.Abstractions.Network;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Controllers;

/// <summary>
/// Контроллер матча
/// </summary>
public class MatchController : IMatchController
{
    private readonly Queue<CardData> _deck;
    private readonly IRound _round;
    private MatchData _data;

    public MatchData Data => _data;

    /// <summary>
    /// Ивент обновления матча
    /// </summary>
    public event Action<string> OnMatchUpdated = delegate {};
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomData">Данные комнаты</param>
    public MatchController(RoomData roomData)
    {
        _deck = new Queue<CardData>(ShuffleCardController.ShuffleCards());

        CardData trumpCard = new CardData();

        while (trumpCard.State || trumpCard.Seniority == Seniority.Ace)
        {
            trumpCard = _deck.Dequeue();
            _deck.Enqueue(trumpCard);
        }

        _data = new MatchData(
            new DealerData((byte)_deck.Count, trumpCard),
            new DeskData(new CardData[6], new CardData[6]),
            roomData,
            roomData.Clients.Select(c => new PlayerData(roomData.Config.Id ,c, 0)));

        _round = new RoundNetworkController(roomData, trumpCard);
    }

    /// <summary>
    /// Обновить данные игрока
    /// </summary>
    /// <param name="playerData">Данные игрока</param>
    public void UpdatePlayerData(PlayerData playerData)
    {
        for (var i = 0; i < Data.Players.Count; i++)
            if (string.Equals(Data.Players[i].Data.Id, playerData.Data.Id))
                Data.Players[i] = playerData;
        
        OnMatchUpdated.Invoke(_data.Room.Config.Id);
    }
    
    public void UpdateDeskData(DeskData deskData)
    {
        var temp = _data;
        temp.Desk = deskData;
        _data = temp;
        
        OnMatchUpdated.Invoke(_data.Room.Config.Id);
    }

    /// <summary>
    /// Получить карты
    /// </summary>
    /// <param name="playerData">Данные игрока</param>
    /// <param name="number">Количество карт сколько нужно получить</param>
    /// <param name="cards">Карты</param>
    /// <returns>Получилось выдать карты?</returns>
    public bool GetCards(PlayerData playerData, byte number, out IEnumerable<CardData> cards)
    {
        if (playerData.NumberCard + number > 6)
        {
            cards = default;
            return false;
        }

        cards = GetCards(Math.Min(number, _data.Dealer.NumberCardsInDeck)).ToList();
        var desk = _data.Dealer;
        desk.NumberCardsInDeck = (byte)_deck.Count;
        _data.Dealer = desk;
        
        OnMatchUpdated.Invoke(_data.Room.Config.Id);

        if (_round.IsThrowerIdentified == false)
            _round.FindFirstThrowerPlayer(playerData.Data.Id, cards);
        
        return true;
    }

    private IEnumerable<CardData> GetCards(byte number)
    {
        for (var i = 0; i < number; i++)
            yield return _deck.Dequeue();
    }

    public IEnumerable<string> ClientIds()
    {
        return Data.Players.Select(c => c.Data.Id);
    }
}