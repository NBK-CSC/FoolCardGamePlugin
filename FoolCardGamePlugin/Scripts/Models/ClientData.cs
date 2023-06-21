﻿using System;
using DarkRift;

namespace FoolCardGamePlugin.Models;

/// <summary>
/// Данные клиента
/// </summary>
public struct ClientData : IDarkRiftSerializable
{
    public string Id;
    public bool State;

    public ClientData() : this("", false) { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="state">Состояние</param>
    public ClientData(string id, bool state)
    {
        Id = id;
        State = state;
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        Id = e.Reader.ReadString();
        State = e.Reader.ReadBoolean();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Id);
        e.Writer.Write(State);
    }
}