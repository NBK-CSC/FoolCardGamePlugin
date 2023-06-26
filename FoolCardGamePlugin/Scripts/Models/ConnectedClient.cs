using DarkRift.Server;
using FoolCardGamePlugin.Abstractions.Repositories;

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
    /// <param name="clientRepository">Хранилище клиентов</param>
    public ConnectedClient(IClient client, IRepository<string, ConnectedClient> clientRepository)
    {
        Client = client;
        Data = new ClientData(client.ID.ToString(), false);
        IsInRoom = false;
        IsInMatch = false;
        
        clientRepository.Entities.Add(Data.Id, this);
    }
}