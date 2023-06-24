using System.Collections.Generic;
using System.Linq;
using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Abstractions.Network;
using FoolCardGamePlugin.Controllers;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

public class MatchNetworkController : IMatchCreating, IMatchStopping
{
    private readonly MatchesController _matchesController;

    public MatchNetworkController()
    {
        _matchesController = new MatchesController();
    }
    
    public bool TryCreateMatch(RoomData roomData)
    {
        if (_matchesController.TryCreateMatch(roomData, UpdateDesk))
            return false;
        
        SetClientStatusOnMatch(roomData.Clients, true);
        return true;
    }
    
    public void GetCards(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;
        
        var getCardsData = NetworkReader.Instance.Read<GetCardsData>(e);
        if (_matchesController.GetCards(ref getCardsData) == false)
            return;
        
        NetworkSender.Instance.SendRequest(Tags.GetCards, client.Client, getCardsData);
    }
    
    private void SetClientStatusOnMatch(IEnumerable<ClientData> clients, bool status)
    {
        foreach (var client in clients)
            if (ServerManager.Instance.Clients.ContainsKey(client.Id))
                ServerManager.Instance.Clients[client.Id].IsInMatch = status;
    }
    
    public void UpdatePlayerData(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;
        
        _matchesController.UpdatePlayerData(NetworkReader.Instance.Read<PlayerData>(e));
    }

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
        
        SendMessagePlayers(roomId, Tags.UpdateMatch, _matchesController[roomId].Data);
    }

    private void SendMessagePlayers<T>(string roomId, Tags tag, T data) where T : IDarkRiftSerializable
    {
        foreach (var clientId in _matchesController[roomId].ClientIds())
            NetworkSender.Instance.SendRequest(tag, ServerManager.Instance.Clients[clientId].Client, data);
    }
    
    private void SendMessagePlayers(string roomId, Tags tag)
    {
        foreach (var clientId in _matchesController[roomId].ClientIds())
            NetworkSender.Instance.SendRequest(tag, ServerManager.Instance.Clients[clientId].Client);
    }

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
        
        SendMessagePlayers(roomId, Tags.StopRound);
    }
}