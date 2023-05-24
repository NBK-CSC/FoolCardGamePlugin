using DarkRift;
using DarkRift.Server;

namespace FoolCardGamePlugin.Network;

/// <summary>
/// Сетевой чтец
/// </summary>
public class NetworkReader
{
    private static NetworkReader _instance;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static NetworkReader Instance => _instance ??= new NetworkReader();
    
    /// <summary>
    /// Прочитать сообщение
    /// </summary>
    /// <param name="e">Ивент сообщения</param>
    /// <typeparam name="T">Тип </typeparam>
    /// <returns></returns>
    public T Read<T>(MessageReceivedEventArgs e) where T : IDarkRiftSerializable, new()
    {
        using (var message = e.GetMessage())
        {
            return message.Deserialize<T>();
        }
    }
}