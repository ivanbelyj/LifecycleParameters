using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Storage that allows to sync lifecycle parameters of an entity in multiplayer
/// </summary>
public class ParameterStorage : NetworkBehaviour, IParameterStorage
{
    private readonly SyncDictionary<string, float> syncValues = new();

    public float GetParameterValue(string parameterId)
    {
        return syncValues[parameterId];
    }

    [Server]
    public void SetParameterValue(string parameterId, float value)
    {
        syncValues[parameterId] = value; 
    }
}
