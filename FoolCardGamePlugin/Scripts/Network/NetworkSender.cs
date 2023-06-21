using DarkRift;
using DarkRift.Server;
using FoolCardGamePlugin.Network.Enums;

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
    
    public void SendRequest<T>(Tags tag, IClient client, T data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }

    public void SendRequest<T>(Tags tag, IClient client, T[] data, SendMode sendMode = SendMode.Reliable) where T : IDarkRiftSerializable
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(data);
            SendMessage(tag, client, writer, sendMode);
        }
    }
    
    public void SendRequest(Tags tag, IMessageSinkSource sinkSource, SendMode sendMode = SendMode.Reliable)
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