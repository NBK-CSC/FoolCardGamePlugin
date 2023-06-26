using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Network.Enums;
using FoolCardGamePlugin.Repositories;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Network отправитель
/// </summary>
public class NetworkSender
{
    private static NetworkSender _instance;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static NetworkSender Instance => _instance ??= new NetworkSender();
    
    public void SendResponse<T>(Tags tag, IClient client, T data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }

    public void SendResponse<T>(Tags tag, IClient client, T[] data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }
    
    public void SendResponse<T>(Tags tag, IEnumerable<IClient> clients, T data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        foreach (var client in clients)
            SendResponse<T>(tag, client, data, sendMode);
    }
    
    public void SendResponse<T>(Tags tag, IEnumerable<IClient> clients, T[] data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        foreach (var client in clients)
            SendResponse<T>(tag, client, data, sendMode);
    }
    
    public void SendResponse(Tags tag, IEnumerable<IClient> clients, SendMode sendMode = SendMode.Reliable)
    {
        foreach (var client in clients)
            SendResponse(tag, client, sendMode);
    }
    
    public void SendResponse<T>(Tags tag, IEnumerable<string> clientsIds, T[] data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        foreach (var clientId in clientsIds)
            if(ServerManager.Instance.Clients.ContainsKey(clientId))
                SendResponse<T>(tag, ServerManager.Instance.Clients[clientId].Client, data, sendMode);
    }
    
    public void SendResponse<T>(Tags tag, IEnumerable<string> clientsIds, T data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        foreach (var clientId in clientsIds)
            if(ServerManager.Instance.Clients.ContainsKey(clientId))
                SendResponse<T>(tag, ServerManager.Instance.Clients[clientId].Client, data, sendMode);
    }
    
    public void SendResponse(Tags tag, IEnumerable<string> clientsIds, SendMode sendMode = SendMode.Reliable)
    {
        foreach (var clientId in clientsIds)
            if(ServerManager.Instance.Clients.ContainsKey(clientId))
                SendResponse(tag, ServerManager.Instance.Clients[clientId].Client, sendMode);
    }
    
    public void SendResponse(Tags tag, IMessageSinkSource sinkSource, SendMode sendMode = SendMode.Reliable)
    {
        using (Message message = Message.CreateEmpty((ushort)tag))
        {
            sinkSource.SendMessage(message, sendMode);
        }
    }
    
    private void SendMessage(Tags tag, IMessageSinkSource sinkSource, DarkRiftWriter writer, SendMode sendMode)
    {
        using (var message = Message.Create((ushort)tag, writer))
        {
            sinkSource.SendMessage(message, sendMode);
        }
    }
}