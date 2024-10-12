using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLifecycleParametersProvider :
    MonoBehaviour,
    IInitialParametersProvider,
    IInitialEffectsProvider
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField]
    private LifecycleParameterData health;

    [SerializeField]
    private LifecycleParameterData endurance;
    
    [SerializeField]
    private LifecycleParameterData satiety;
    
    [SerializeField]
    private LifecycleParameterData bleed;
    
    [SerializeField]
    private LifecycleParameterData radiation;
    #endregion

    #region Effects
    [Header("Initial effects")]
    [SerializeField]
    private LifecycleEffect regeneration;
    [SerializeField]
    private LifecycleEffect enduranceRecovery;
    [SerializeField]
    private LifecycleEffect hunger;
    [SerializeField]
    private LifecycleEffect radiationExcretion;

    [Header("Temporary effects")]
    [SerializeField]
    private LifecycleEffect runEnduranceDecrease;
    [SerializeField]
    private LifecycleEffect bleedEffect;
    [SerializeField]
    private LifecycleEffect radiationPoisoning;
    #endregion

    public LifecycleEffect[] GetInitialEffects()
    {
        var initialEffects = new LifecycleEffect[] {
            regeneration,
            enduranceRecovery,
            hunger,
            radiationExcretion };
        return initialEffects;
    }

    public LifecycleParameterData[] GetInitialParameters()
    {
        var initialParameters = new LifecycleParameterData[] {
            health,
            endurance,
            satiety,
            bleed,
            radiation,
        };
        return initialParameters;
    }
}
