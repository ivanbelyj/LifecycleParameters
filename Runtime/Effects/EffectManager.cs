using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EffectManager : NetworkBehaviour
{
    private readonly SyncHashSet<LifecycleEffect> syncEffects = new();

    private readonly HashSet<LifecycleEffect> effects = new();

    ///<summary>
    /// All uncompleted effects applied to change the parameters of the lifecycle.
    /// Completed effects are removed after regular traversal 
    ///</summary>
    public HashSet<LifecycleEffect> Effects => effects;

    public virtual void Awake() {
        syncEffects.Callback += SyncEffects;
    }

    public override void OnStartClient() {
        // When connecting a player on the server, there could already be effects
        foreach (var effect in syncEffects) {
            effects.Add(effect);
        }
    }    

    public void RemoveEffect(LifecycleEffect effect) {
        if (isServer) {
            RemoveLifecycleEffect(effect);
        } else {
            CmdRemoveLifecycleEffect(effect);
        }
    }

    /// <summary>
    /// Adds an effect and returns the same effect, but setting start time
    /// (which was added)
    /// </summary>
    public LifecycleEffect AddEffectAndSetStartTime(LifecycleEffect effect) {
        effect.StartTime = NetworkTime.time;
        // Todo: not change passed effect
        if (isServer) {
            AddLifecycleEffect(effect);
        } else {
            CmdAddLifecycleEffect(effect);
        }
        return effect;
    }

    #region Sync
    private void SyncEffects(
        SyncHashSet<LifecycleEffect>.Operation op,
        LifecycleEffect item) {
        switch (op) {
            case SyncHashSet<LifecycleEffect>.Operation.OP_ADD:
            {
                effects.Add(item);
                break;
            }
            case SyncHashSet<LifecycleEffect>.Operation.OP_REMOVE:
            {
                effects.Remove(item);
                break;
            }
        }
    }

    [Server]
    public void RemoveLifecycleEffect(LifecycleEffect effect) {
        syncEffects.Remove(effect);
    }

    [Command]
    private void CmdRemoveLifecycleEffect(LifecycleEffect effect) {
        RemoveLifecycleEffect(effect);
    }

    [Server]
    private void AddLifecycleEffect(LifecycleEffect effect) {
        syncEffects.Add(effect);
    }

    [Command]
    private void CmdAddLifecycleEffect(LifecycleEffect effect) {
        AddLifecycleEffect(effect);
    }
    #endregion
}
