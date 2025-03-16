using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static LivingEntityLifecycleParameterIds;

public static class LivingEntityLifecycleParameterIds
{
    public const string Endurance = nameof(Endurance);
    public const string Satiety = nameof(Satiety);
    public const string Bleed = nameof(Bleed);
    public const string Radiation = nameof(Radiation);
}

public class LivingEntityParametersProvider :
    MonoBehaviour,
    IInitialParametersProvider,
    IInitialEffectsProvider
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField]
    private LifecycleParameterData health = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = LifecycleParameterIds.Integrity,
        InitialValue = 1,
        RecoveredValue = 1
    };

    [SerializeField]
    private LifecycleParameterData endurance = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = Endurance,
        InitialValue = 1,
        RecoveredValue = 1
    };
    
    [SerializeField]
    private LifecycleParameterData satiety = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = Satiety,
        InitialValue = 1,
        RecoveredValue = 0
    };
    
    [SerializeField]
    private LifecycleParameterData bleed = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = Bleed,
        InitialValue = 0,
        RecoveredValue = 0
    };
    
    [SerializeField]
    private LifecycleParameterData radiation = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = Radiation,
        InitialValue = 0,
        RecoveredValue = 0
    };
    #endregion

    #region Effects
    [Header("Initial effects")]
    [SerializeField]
    private LifecycleEffect regeneration = new() {
        isInfinite = true,
        targetParameterId = LifecycleParameterIds.Integrity,
        speed = 0.02f
    };

    [SerializeField]
    private LifecycleEffect enduranceRecovery = new() {
        isInfinite = true,
        targetParameterId = Endurance,
        speed = 0.01f
    };

    [SerializeField]
    private LifecycleEffect hunger = new() {
        isInfinite = true,
        targetParameterId = Satiety,
        speed = -0.001f
    };

    [SerializeField]
    private LifecycleEffect radiationExcretion = new() {
        isInfinite = true,
        targetParameterId = Radiation,
        speed = -0.001f
    };
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
