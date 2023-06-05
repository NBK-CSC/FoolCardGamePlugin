using System.Collections.Generic;
using System.Linq;
using FoolCardGamePlugin.Network;

namespace FoolCardGamePlugin.Controllers;

/// <summary>
/// Контроллер комнаты
/// </summary>
public class RoomController
{
    private RoomConfig _config;
    private RoomData _data;
    private List<ClientData> _clients;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="config">Конфиг комнаты</param>
    public RoomController(RoomConfig config)
    {
        _config = config;
        _data = new RoomData(config);

        _clients = new List<ClientData>(config.MaxSlots);
        
        for (int i = 0; i < config.MaxSlots; i++)
            _clients.Add(new ClientData());
    }

    /// <summary>
    /// Пуста ли комната?
    /// </summary>
    public bool IsEmpty => _clients.All(x => string.IsNullOrEmpty(x.Id));

    /// <summary>
    /// Попытка добавить клиента
    /// </summary>
    /// <param name="clientData">Инфа клиента</param>
    /// <returns>Получилось ли добавить клиента?</returns>
    public bool TryAddClient(ClientData clientData)
    {
        for (int i = 0; i < _clients.Count; i++)
        {
            if (!string.IsNullOrEmpty(_clients[i].Id)) 
                continue;
            
            _clients[i] = clientData;
            _config.Slots++;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Попытка удалить клиента
    /// </summary>
    /// <param name="clientData">Инфа клиента</param>
    /// <returns>Получилось ли удалить?</returns>
    public bool TryRemoveClient(ClientData clientData)
    {
        for (int i = 0; i < _clients.Capacity; i++)
        {
            if (string.Equals(_clients[i].Id,clientData.Id))
            {
                _clients[i] = new ClientData();
                _config.Slots--;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Получить инфу комнаты
    /// </summary>
    /// <returns>Инфа комнаты</returns>
    public RoomData GetData()
    {
        _data.Config = _config;
        _data.Clients = _clients;
        
        return _data;
    }
    
    /// <summary>
    /// Получить конфиг комнаты
    /// </summary>
    /// <returns>Конфиг комнаты</returns>
    public RoomConfig GetConfig()
    {
        return _config;
    }
}