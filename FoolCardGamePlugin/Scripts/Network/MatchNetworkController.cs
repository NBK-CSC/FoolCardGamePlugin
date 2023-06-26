using System;
using System.Collections.Generic;
using DarkRift.Server;
using FoolCardGamePlugin.Abstractions.Network;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;
using FoolCardGamePlugin.Repositories;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Сетевой контроллер матча
/// </summary>
public class MatchNetworkController : IMatchCreating, IMatchStopping
{
    private readonly MatchesController _matchesController;

    /// <summary>
    /// Конструктор
    /// </summary>
    public MatchNetworkController()
    {
        _matchesController = new MatchesController();
    }
    
    public bool TryCreateMatch(RoomData roomData)
    {
        if (_matchesController.TryCreateMatch(roomData, UpdateDesk) == false)
            return false;
        
        SetClientStatusOnMatch(roomData.Clients, true);
        return true;
    }
    
    /// <summary>
    /// Выдать карты
    /// </summary>
    /// <param name="client">Клиент</param>
    /// <param name="e">Сообщение</param>
    public void GetCards(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;
        
        var getCardsData = NetworkReader.Instance.Read<GetCardsData>(e);
        if (_matchesController.GetCards(ref getCardsData) == false)
            return;
        
        NetworkSender.Instance.SendResponse(Tags.GetCards, client.Client, getCardsData);
    }
    
    private void SetClientStatusOnMatch(IEnumerable<ClientData> clients, bool status)
    {
        foreach (var client in clients)
            if (ServerManager.Instance.Clients.ContainsKey(client.Id))
                ServerManager.Instance.Clients[client.Id].IsInMatch = status;
    }
    
    /// <summary>
    /// Обновить данные игрока
    /// </summary>
    /// <param name="client">Клиент</param>
    /// <param name="e">Сообщение</param>
    public void UpdatePlayerData(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;
        
        _matchesController.UpdatePlayerData(NetworkReader.Instance.Read<PlayerData>(e));
    }

    /// <summary>
    /// Обновить матч
    /// </summary>
    /// <param name="client">Клиент</param>
    /// <param name="e">Сообщение</param>
    public void UpdateMatch(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;

        UpdateDesk(NetworkReader.Instance.Read<RoomConfig>(e).Id);
    }

    private void UpdateDesk(string roomId)
    {
        if (_matchesController[roomId] == null)
            return;
        
        NetworkSender.Instance.SendResponse(Tags.UpdateMatch, _matchesController[roomId].ClientIds(), _matchesController[roomId].Data);
    }

    /// <summary>
    /// Обновить матч
    /// </summary>
    /// <param name="roomId">Id комнаты</param>
    public void StopMatch(string roomId)
    {
        if (_matchesController[roomId] == null)
            return;

        SetClientStatusOnMatch(_matchesController[roomId].Data.Room.Clients, false);
        _matchesController.RemoveMatch(roomId, UpdateDesk);
    }

    public void StopRound(string roomId)
    {
        if (_matchesController[roomId] == null)
            return;
        
        
    }
}