using System.Collections.Generic;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Abstractions.Network;

/// <summary>
/// Интерфейс контроллера матча
/// </summary>
public interface IMatchController
{
    /// <summary>
    /// Данные матча
    /// </summary>
    public MatchData Data { get; }

    /// <summary>
    /// Список id клиентов
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> ClientIds();
}