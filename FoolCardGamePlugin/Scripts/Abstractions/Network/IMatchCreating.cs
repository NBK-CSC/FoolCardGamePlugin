using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Abstractions.Network;

/// <summary>
/// Интерфейс создания матча
/// </summary>
public interface IMatchCreating
{
    /// <summary>
    /// Попытаться создать матч
    /// </summary>
    /// <param name="roomData">Данные комнаты</param>
    /// <returns>Получилось ли создать?</returns>
    public bool TryCreateMatch(RoomData roomData);
}