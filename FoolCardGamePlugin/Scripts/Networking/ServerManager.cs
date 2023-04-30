using System.Collections.Generic;

namespace FoolCardGamePlugin.Networking;

/// <summary>
/// Менеджер сервера
/// </summary>
public class ServerManager
{
    private static ServerManager _instance;

    private Dictionary<int, ConnectedClient> _clients;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static ServerManager Instance => _instance ??= new ServerManager();

    /// <summary>
    /// Словарь клиентов
    /// </summary>
    public IDictionary<int, ConnectedClient> Clients => _clients;

    private ServerManager()
    {
        _clients = new Dictionary<int, ConnectedClient>();
    }
}