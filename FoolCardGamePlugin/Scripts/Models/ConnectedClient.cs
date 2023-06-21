using DarkRift.Server;
using FoolCardGamePlugin.Network;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Класс подключенного клиента
/// </summary>
public class ConnectedClient
{
    public IClient Client;
    public ClientData Data;
    public bool IsInRoom;
    public bool IsInMatch;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Ссылка на клиент</param>
    public ConnectedClient(IClient client)
    {
        Client = client;
        Data = new ClientData(client.ID.ToString(), false);
        IsInRoom = false;
        IsInMatch = false;
        
        ServerManager.Instance.Clients.Add(Data.Id, this);
    }
}