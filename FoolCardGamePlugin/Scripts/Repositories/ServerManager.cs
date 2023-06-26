using System.Collections.Generic;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Repositories;

/// <summary>
/// Менеджер сервера
/// </summary>
public class ServerManager
{
    private static ServerManager _instance;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static ServerManager Instance => _instance ??= new ServerManager();

    /// <summary>
    /// Словарь клиентов
    /// </summary>
    public IReadOnlyDictionary<string, ConnectedClient> Clients => ClientRepository.Instance.Entities;

    private ServerManager() { }
}