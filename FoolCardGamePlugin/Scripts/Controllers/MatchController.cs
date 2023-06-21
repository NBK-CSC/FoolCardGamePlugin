using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network;

namespace FoolCardGamePlugin.Controllers;

/// <summary>
/// Контроллер матча
/// </summary>
public class MatchController
{
    private readonly Queue<CardData> _deck;
    private readonly RoundController _roundController;
    private MatchData _data;

    public MatchData Data => _data;

    public event Action<string> OnMatchUpdated = delegate {};
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomData">Данные комнаты</param>
    public MatchController(RoomData roomData)
    {
        _deck = new Queue<CardData>(ShuffleCardController.ShuffleCards());
        var trumpCard = _deck.Dequeue();
        _deck.Enqueue(trumpCard);

        _data = new MatchData(
            new DeskData((byte)_deck.Count, trumpCard),
            roomData,
            roomData.Clients.Select(c => new PlayerData(roomData.Config.Id ,c, 0)));

        _roundController = new RoundController(roomData.Config, trumpCard);
    }

    public void UpdatePlayerData(PlayerData playerData)
    {
        for (var i = 0; i < Data.Players.Count; i++)
            if (string.Equals(Data.Players[i].Data.Id, playerData.Data.Id))
                Data.Players[i] = playerData;
        OnMatchUpdated.Invoke(_data.Room.Config.Id);
    }

    public bool GetCards(PlayerData playerData, byte number, out IEnumerable<CardData> cards)
    {
        if (playerData.NumberCard + number > 6)
        {
            cards = default;
            return false;
        }

        cards = GetCards(number).ToList();

        var desk = _data.Desk;
        desk.NumberCardsInDeck = (byte)_deck.Count;
        _data.Desk = desk;
        
        OnMatchUpdated.Invoke(_data.Room.Config.Id);

        if (_roundController.IsDefenderIdentified == false)
            _roundController.FindDefenderPlayer(playerData.Data.Id, cards);
        
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