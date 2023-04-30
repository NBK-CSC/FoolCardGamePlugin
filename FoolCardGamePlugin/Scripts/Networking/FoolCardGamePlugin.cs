using System;
using DarkRift.Server;
using FoolCardGamePlugin.Networking.Enums;

namespace FoolCardGamePlugin.Networking;

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
        if (ServerManager.Instance.Clients.ContainsKey(client.ID) == false)
            new ConnectedClient((IClient)sender);
        switch (e.Tag)
        {
            case (ushort)Tags.CreateRoom:
                RoomMediator.Instance.Create(ServerManager.Instance.Clients[client.ID], e);
                break;
            case (ushort)Tags.GetRooms:
                //TODO отправку комнат
                break;
        }
    }
}