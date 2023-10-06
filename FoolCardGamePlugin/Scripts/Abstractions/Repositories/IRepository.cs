using System.Collections.Generic;

namespace FoolCardGamePlugin.Abstractions.Repositories;

/// <summary>
/// Интерфейс хранилища
/// </summary>
/// <typeparam name="TKey">Ключ</typeparam>
/// <typeparam name="TValue">Значение</typeparam>
public interface IRepository<TKey, TValue>
{
    /// <summary>
    /// Сущности
    /// </summary>
    public Dictionary<TKey, TValue> Entities { get; }
}