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
    private readonly Dictionary<string, MatchController> _matches = new ();
    
    public bool TryCreateMatch(RoomData roomData)
    {
        if (_matches.ContainsKey(roomData.Config.Id))
            return false;
        
        var match = new MatchController(roomData);
        match.OnMatchUpdated += UpdateDesk;
        
        _matches.Add(roomData.Config.Id, match);
        SetClientStatusOnMatch(roomData.Clients, true);
        
        return true;
    }
    
    public void GetCards(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;
        
        var getCardsData = NetworkReader.Instance.Read<GetCardsData>(e);
        if (_matches.ContainsKey(getCardsData.PlayerData.RoomId) == false)
            return;

        if (_matches[getCardsData.PlayerData.RoomId].GetCards(getCardsData.PlayerData, getCardsData.Number, out var cards))
            getCardsData.Cards = cards.ToArray();
        
        NetworkSender.Instance.SendRequest(Tags.GetCards, client.Client, getCardsData);
        //UpdateDesk(getCardsData.PlayerData.RoomId);
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
        
        var playerData = NetworkReader.Instance.Read<PlayerData>(e);
        _matches[playerData.RoomId].UpdatePlayerData(playerData);
        //UpdateDesk(playerData.RoomId);
    }

    public void UpdateMatch(ConnectedClient client, MessageReceivedEventArgs e)
    {
        if (client.IsInRoom == false || client.IsInMatch == false)
            return;

        UpdateDesk(NetworkReader.Instance.Read<RoomConfig>(e).Id);
    }

    private void UpdateDesk(string roomId)
    {
        if (_matches.ContainsKey(roomId) == false)
            return;
        
        SendMessagePlayers(roomId, Tags.UpdateMatch, _matches[roomId].Data);
    }

    private void SendMessagePlayers<T>(string roomId, Tags tag, T data) where T : IDarkRiftSerializable
    {
        foreach (var clientId in _matches[roomId].ClientIds())
            NetworkSender.Instance.SendRequest(tag, ServerManager.Instance.Clients[clientId].Client, data);
    }
    
    private void SendMessagePlayers(string roomId, Tags tag)
    {
        foreach (var clientId in _matches[roomId].ClientIds())
            NetworkSender.Instance.SendRequest(tag, ServerManager.Instance.Clients[clientId].Client);
    }

    public void StopMatch(string roomId)
    {
        if (_matches.ContainsKey(roomId) == false)
            return;

        SetClientStatusOnMatch(_matches[roomId].Data.Room.Clients, false);
        _matches[roomId].OnMatchUpdated -= UpdateDesk;
        _matches.Remove(roomId);
    }

    public void StopRound(string roomId)
    {
        if (_matches.ContainsKey(roomId) == false)
            return;
        
        SendMessagePlayers(roomId, Tags.StopRound);
    }
}