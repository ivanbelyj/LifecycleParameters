using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

using static LifecycleParameterIds;

public class DestroyableLifecycle : EntityLifecycleBase
{
    public event Action EntityDestroyed;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var health = parameterManager.GetParameter(Integrity);

        health.MinReached += OnDestroyEntity;

        // Deathly initial value does not work - it looks like 
        // the players are on the clients not created yet
        // if (health.Value <= health.MinValue) {
        //     OnDestroyEntity();
        // }
    }

    [Server]
    private void OnDestroyEntity() {
        DestroyEntity();
        RpcDestroyEntity();
    }

    [ClientRpc]
    private void RpcDestroyEntity() {
        DestroyEntity();
    }

    public virtual void DestroyEntity() {
        EntityDestroyed?.Invoke();
    }
}
