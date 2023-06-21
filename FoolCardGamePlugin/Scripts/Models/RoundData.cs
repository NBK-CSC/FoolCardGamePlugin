using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные раунда
/// </summary>
public struct RoundData : IDarkRiftSerializable
{
    public string DefenderPlayerId;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="defenderPlayerId">Id защищающегося игрока</param>
    public RoundData(string defenderPlayerId)
    {
        DefenderPlayerId = defenderPlayerId;
    }

    public void Deserialize(DeserializeEvent e)
    {
        DefenderPlayerId = e.Reader.ReadString();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(DefenderPlayerId);
    }
}