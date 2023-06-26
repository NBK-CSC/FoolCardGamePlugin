using System;
using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Abstractions.Controllers;
using FoolCardGamePlugin.Controllers;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Сетевой контроллер раунда
/// </summary>
public class RoundNetworkController : IRound, IDisposable
{
    private readonly RoundController _roundController;
    private string _defenderPlayerId;
    private RoomData _roomData;

    public bool IsThrowerIdentified => _roundController.IsThrowerIdentified;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roomData">Данные комнаты</param>
    /// <param name="trumpCard">Козырная карта</param>
    public RoundNetworkController(RoomData roomData, CardData trumpCard)
    {
        _roundController = new RoundController(roomData.Config, trumpCard);
        _roomData = roomData;
        _roundController.OnDefenderSearched += OnDefenderSearch;
    }

    public void FindFirstThrowerPlayer(string playerId, IEnumerable<CardData> cards)
    {
        _roundController.FindFirstThrowerPlayer(playerId, cards);
    }
    
    private void OnDefenderSearch(string id)
    {
        var index = _roomData.Clients.FindIndex(c => string.Equals(c.Id, id));
        _defenderPlayerId = index + 1 == _roomData.Clients.Count ? _roomData.Clients[0].Id : _roomData.Clients[index + 1].Id;
        StartRound(new RoundData(_defenderPlayerId));
    }

    public void StartRound(RoundData data)
    {
        NetworkSender.Instance.SendResponse(Tags.StartRound, _roomData.Clients.Select(c => c.Id), data);
    }

    public void StopRound()
    {
        NetworkSender.Instance.SendResponse(Tags.StopRound, _roomData.Clients.Select(c => c.Id));
    }

    public void Dispose()
    {
        _roundController.OnDefenderSearched -= OnDefenderSearch;
    }
}