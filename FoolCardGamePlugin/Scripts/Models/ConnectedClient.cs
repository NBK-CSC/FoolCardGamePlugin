using DarkRift.Server;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Класс подключенного клиента
/// </summary>
public class ConnectedClient
{
    public IClient Client;
    public ushort ClientId;
    public bool IsInRoom;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Ссылка на клиент</param>
    public ConnectedClient(IClient client)
    {
        Client = client;
        ClientId = client.ID;
        IsInRoom = false;
        
        ServerManager.Instance.Clients.Add(ClientId, this);
    }
}