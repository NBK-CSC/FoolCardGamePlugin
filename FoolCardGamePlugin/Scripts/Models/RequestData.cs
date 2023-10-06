using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные запроса
/// </summary>
/// <typeparam name="T">Тип инфы для передачи через запроса</typeparam>
public struct RequestData<T> : IDarkRiftSerializable where T : IDarkRiftSerializable, new()
{
    public string Sender;
    public string Receiver;
    public string Id;
    public T Data;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="receiver">Получатель</param>
    /// <param name="id">Id запроса</param>
    /// <param name="data">Данные запроса</param>
    public RequestData(string sender, string receiver, string id, T data)
    {
        Sender = sender;
        Receiver = receiver;
        Id = id;
        Data = data;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Sender = e.Reader.ReadString();
        Receiver = e.Reader.ReadString();
        Id = e.Reader.ReadString();
        Data = e.Reader.ReadSerializable<T>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Sender);
        e.Writer.Write(Receiver);
        e.Writer.Write(Id);
        e.Writer.Write(Data);
    }
}
