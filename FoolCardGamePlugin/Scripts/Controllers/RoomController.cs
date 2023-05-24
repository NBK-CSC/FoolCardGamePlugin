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

    public RoomController(RoomConfig config)
    {
        _config = config;
        _data = new RoomData(config);

        _clients = new List<ClientData>(config.MaxSlots);
        
        for (int i = 0; i < config.MaxSlots; i++)
            _clients.Add(new ClientData());
    }

    public bool IsEmpty => _clients.Any(x => x.Id == null);

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

    public bool TryRemoveClient(ClientData clientData)
    {
        for (int i = 0; i < _clients.Capacity; i++)
        {
            if (string.Equals(_clients[i].Id,clientData.Id))
            {
                _clients[i] = default;
                _config.Slots--;
                return true;
            }
        }

        return false;
    }

    public RoomData GetData()
    {
        _data.Config = _config;
        _data.Clients = _clients;
        
        return _data;
    }
}