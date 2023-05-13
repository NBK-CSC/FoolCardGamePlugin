using System;
using System.Diagnostics;
using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Network.Enums;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Кастомный плагин
/// </summary>
public class FoolCardGamePlugin : Plugin
{
    public override Version Version => new Version(0, 0, 1);
    public override bool ThreadSafe => false;

    public FoolCardGamePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        ClientManager.ClientConnected += OnClientConnected;
        ClientManager.ClientDisconnected += OnClientDisconnected;
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
                RoomNetworkController.Instance.Create(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.GetRooms:
                //RoomNetworkController.Instance.Create(ServerManager.Instance.Clients[id], e);
                break;
            case (ushort)Tags.JoinRoom:
                RoomNetworkController.Instance.JoinRoom(ServerManager.Instance.Clients[id], e);
                break;
        }
    }
}