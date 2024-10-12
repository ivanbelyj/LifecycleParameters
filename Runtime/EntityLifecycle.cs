using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class EntityLifecycle : EntityLifecycleBase
{
    public event Action OnDeath;

    private LifecycleEffect runEffect;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // TODO: don't use hardcoded id
        var health = parameterManager.GetParameter(0);

        health.MinReached += OnDie;

        if (health.Value <= health.MinValue) {
            OnDie();
        }
    }

    [Server]
    private void OnDie() {
        Die();
        Debug.Log("ON DIE (SERVER)");
        RpcDie();
    }

    [ClientRpc]
    private void RpcDie() {
        Debug.Log("RPC DIE");
        Die();
    }

    private void Die() {
        Debug.Log("DIE");
    }
}
