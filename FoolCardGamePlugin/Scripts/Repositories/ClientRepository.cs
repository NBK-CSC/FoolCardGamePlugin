using System.Collections.Generic;
using FoolCardGamePlugin.Abstractions.Repositories;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Repositories;

/// <summary>
/// Хранилище слиентов
/// </summary>
internal class ClientRepository : IRepository<string, ConnectedClient>
{
    private static ClientRepository _instance;

    /// <summary>
    /// Синглтон
    /// </summary>
    internal static ClientRepository Instance => _instance ??= new ClientRepository();

    public Dictionary<string, ConnectedClient> Entities { get; }

    private ClientRepository()
    {
        Entities = new Dictionary<string, ConnectedClient>();
    }
}