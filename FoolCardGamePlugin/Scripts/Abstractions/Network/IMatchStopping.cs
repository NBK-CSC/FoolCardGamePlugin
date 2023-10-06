namespace FoolCardGamePlugin.Abstractions.Network;

/// <summary>
/// Интерфейс остановки матча
/// </summary>
public interface IMatchStopping
{
    /// <summary>
    /// Остановаить матч
    /// </summary>
    /// <param name="roomId"></param>
    public void StopMatch(string roomId);
}