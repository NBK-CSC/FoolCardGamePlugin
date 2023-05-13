using System.Collections.Generic;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Менеджер сервера
/// </summary>
public class ServerManager
{
    private static ServerManager _instance;

    private Dictionary<string, ConnectedClient> _clients;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static ServerManager Instance => _instance ??= new ServerManager();

    /// <summary>
    /// Словарь клиентов
    /// </summary>
    public IDictionary<string, ConnectedClient> Clients => _clients;

    private ServerManager()
    {
        _clients = new Dictionary<string, ConnectedClient>();
    }
}