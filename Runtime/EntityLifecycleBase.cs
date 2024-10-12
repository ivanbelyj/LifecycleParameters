using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// A component of the lifecycle of an entity (a living being, for example)
///</summary>
[RequireComponent(typeof(IParameterStorage))]
[RequireComponent(typeof(EffectManager))]
[RequireComponent(typeof(IInitialEffectsProvider))]
[RequireComponent(typeof(IInitialParametersProvider))]
public abstract class EntityLifecycleBase : NetworkBehaviour
{
    private protected ParameterManager parameterManager;
    protected EffectManager effectManager;
    protected IParameterStorage parameterStorage;

    public virtual void Awake() {
        effectManager = GetComponent<EffectManager>();
        parameterStorage = GetComponent<IParameterStorage>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        parameterManager = new ParameterManager(
            GetInitialParameters(),
            parameterStorage);

        // Setting and synchronizing the initial effects set in the inspector
        foreach (var initialEffect in GetInitialEffects()) {
            effectManager.AddEffectAndSetStartTime(initialEffect);
        }
    }

    public LifecycleEffect AddEffectAndSetStartTime(LifecycleEffect effect) {
        return effectManager.AddEffectAndSetStartTime(effect);
    }

    public void RemoveEffect(LifecycleEffect effect) {
        effectManager.RemoveEffect(effect);
    }

    public float GetParameterValue(uint parameterId) {
        return parameterStorage.GetParameterValue(parameterId);
    }

    private void Update() {
        if (isServer) {
            UpdateEffects();
        }
    }

    [Server]
    private void UpdateEffects() {
        var effectsToRemove = new List<LifecycleEffect>();
        foreach (var effect in effectManager.Effects) {
            if (!effect.isInfinite && effect.IsPassed) {
                // Past temporary effects are postponed for deletion
                // (can't change the dictionary while we're going through it)
                effectsToRemove.Add(effect);
            } else {
                parameterManager.ApplyEffect(effect);
            }
        }

        foreach (var effectId in effectsToRemove) {
            effectManager.RemoveLifecycleEffect(effectId);
        }
    }

    private LifecycleParameter[] GetInitialParameters()
    {
        return CreateLifecycleParameters(
            GetComponent<IInitialParametersProvider>().GetInitialParameters());
    }

    private LifecycleEffect[] GetInitialEffects()
    {
        return GetComponent<IInitialEffectsProvider>().GetInitialEffects();
    }
    
    private LifecycleParameter[] CreateLifecycleParameters(
        LifecycleParameterData[] data) {
        return data
            .Select(x => new LifecycleParameter(x))
            .ToArray();
    }
}
