using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

///<summary>
/// A component of the lifecycle of an entity (a living being, for example)
///</summary>
[RequireComponent(typeof(IParameterStorage))]
[RequireComponent(typeof(EffectManager))]
[RequireComponent(typeof(IInitialEffectsProvider))]
[RequireComponent(typeof(IInitialParametersProvider))]
public abstract class EntityLifecycleBase : NetworkBehaviour
{
    public event EventHandler<List<string>> Initialized;
    private protected ParameterManager parameterManager;
    protected EffectManager effectManager;
    protected IParameterStorage parameterStorage;

    public virtual void Awake() {
        effectManager = GetComponent<EffectManager>();
        parameterStorage = GetComponent<IParameterStorage>();

        // Todo: use one method
        if (GetComponent<IInitialEffectsProvider>() == null) {
            Debug.LogError(
                $"{nameof(IInitialEffectsProvider)} is required for {nameof(EntityLifecycleBase)}. " +
                $"Consider adding it to the game object (name: {gameObject.name})");
        }
        if (GetComponent<IInitialParametersProvider>() == null) {
            Debug.LogError(
                $"{nameof(IInitialParametersProvider)} is required for {nameof(EntityLifecycleBase)}. " +
                $"Consider adding it to the game object (name: {gameObject.name})");
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        parameterManager = new ParameterManager(
            GetInitialParameters(),
            parameterStorage);

        // Setting and synchronizing the initial effects set in the inspector
        foreach (var initialEffect in GetInitialEffects()) {
            effectManager.AddEffect(initialEffect);
        }

        Initialized?.Invoke(this, GetAllParameterIds().ToList());
    }

    public void AddEffect(LifecycleEffect effect) {
        effectManager.AddEffect(effect);
    }

    public void RemoveEffect(LifecycleEffect effect) {
        effectManager.RemoveEffect(effect);
    }

    public float GetParameterValue(string parameterId) {
        return parameterStorage.GetParameterValue(parameterId);
    }

    public IEnumerable<string> GetAllParameterIds()
        => parameterManager.GetAllParameterIds();

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

        foreach (var effect in effectsToRemove) {
            effectManager.RemoveEffect(effect);
        }
    }

    private LifecycleParameter[] GetInitialParameters()
    {
        return CreateLifecycleParameters(
            GetComponent<IInitialParametersProvider>()?.GetInitialParameters()
            ?? Array.Empty<LifecycleParameterData>());
    }

    private LifecycleEffect[] GetInitialEffects()
    {
        return GetComponent<IInitialEffectsProvider>()?.GetInitialEffects()
            ?? Array.Empty<LifecycleEffect>();
    }
    
    private LifecycleParameter[] CreateLifecycleParameters(
        LifecycleParameterData[] data) {
        return data
            .Select(x => new LifecycleParameter(x))
            .ToArray();
    }
}
