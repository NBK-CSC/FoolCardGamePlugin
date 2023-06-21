using System;
using DarkRift.Server;
using FoolCardGamePlugin.Models;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Кастомный плагин
/// </summary>
public class FoolCardGamePlugin : Plugin
{
    public override Version Version => new Version(0, 0, 1);
    public override bool ThreadSafe => false;

    private readonly RoomNetworkController _roomNetworkController;
    private readonly MatchNetworkController _matchNetworkController;

    public FoolCardGamePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        ClientManager.ClientConnected += OnClientConnected;
        ClientManager.ClientDisconnected += OnClientDisconnected;

        _matchNetworkController = new MatchNetworkController();
        _roomNetworkController = new RoomNetworkController(_matchNetworkController, _matchNetworkController);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ClientManager.ClientConnected -= OnClientConnected;
            ClientManager.ClientDisconnected -= OnClientDisconnected;
        }

        base.Dispose(disposing);
    }

    private void OnClientConnected(object? sender, ClientConnectedEventArgs e)
    {
        e.Client.MessageReceived += OnClientMessageReceived;
    }

    private void OnClientDisconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        if (ServerManager.Instance.Clients.ContainsKey(e.Client.ID.ToString()))
        {
            _roomNetworkController.LeaveRoom(ServerManager.Instance.Clients[e.Client.ID.ToString()]);
            ServerManager.Instance.Clients.Remove(e.Client.ID.ToString());
        }
        e.Client.MessageReceived -= OnClientMessageReceived;
    }

    private void OnClientMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        IClient client = (IClient)sender;
        string id = client.ID.ToString();
        
        if (ServerManager.Instance.Clients.ContainsKey(id) == false)
            new ConnectedClient((IClient)sender);
        
        switch (e.Tag)
        {
            case (ushort)Tags.CreateRoom:
                _roomNetworkController.Create(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.GetRooms:
                _roomNetworkController.GetRooms(ServerManager.Instance.Clients[id]);
                break;
            case (ushort)Tags.JoinRoom:
                _roomNetworkController.JoinRoom(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.LeaveRoom:
                _roomNetworkController.LeaveRoom(ServerManager.Instance.Clients[id]);
                break;
            case (ushort)Tags.UpdateClient:
                _roomNetworkController.UpdateClientData(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.GetCards:
                _matchNetworkController.GetCards(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.UpdatePlayer:
                _matchNetworkController.UpdatePlayerData(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.UpdateMatch:
                _matchNetworkController.UpdateMatch(ServerManager.Instance.Clients[id], e);
                break;
        }
    }
}