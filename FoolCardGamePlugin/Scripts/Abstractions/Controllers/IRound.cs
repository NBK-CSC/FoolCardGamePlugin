using System.Collections.Generic;
using FoolCardGamePlugin.Models;

namespace FoolCardGamePlugin.Abstractions.Controllers;

public interface IRound
{
    public bool IsThrowerIdentified { get; }

    public void FindFirstThrowerPlayer(string playerId, IEnumerable<CardData> cards);
}