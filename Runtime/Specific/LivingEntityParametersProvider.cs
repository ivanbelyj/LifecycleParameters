using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityParametersProvider :
    MonoBehaviour,
    IInitialParametersProvider,
    IInitialEffectsProvider
{
    private const uint EnduranceParameterId = 2;
    private const uint SatietyParameterId = 3;
    private const uint BleedParameterId = 4;
    private const uint RadiationParameterId = 5;

    #region Parameters
    [Header("Parameters")]
    [SerializeField]
    private LifecycleParameterData health = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = LifecycleParameterIds.Strength,
        InitialValue = 1,
        RecoveredValue = 1
    };

    [SerializeField]
    private LifecycleParameterData endurance = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = EnduranceParameterId,
        InitialValue = 1,
        RecoveredValue = 1
    };
    
    [SerializeField]
    private LifecycleParameterData satiety = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = SatietyParameterId,
        InitialValue = 1,
        RecoveredValue = 0
    };
    
    [SerializeField]
    private LifecycleParameterData bleed = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = BleedParameterId,
        InitialValue = 0,
        RecoveredValue = 0
    };
    
    [SerializeField]
    private LifecycleParameterData radiation = new() {
        MinValue = 0,
        MaxValue = 1,
        ParameterId = RadiationParameterId,
        InitialValue = 0,
        RecoveredValue = 0
    };
    #endregion

    #region Effects
    [Header("Initial effects")]
    [SerializeField]
    private LifecycleEffect regeneration = new() {
        isInfinite = true,
        targetParameterId = LifecycleParameterIds.Strength,
        speed = 0.02f
    };

    [SerializeField]
    private LifecycleEffect enduranceRecovery = new() {
        isInfinite = true,
        targetParameterId = EnduranceParameterId,
        speed = 0.01f
    };

    [SerializeField]
    private LifecycleEffect hunger = new() {
        isInfinite = true,
        targetParameterId = SatietyParameterId,
        speed = -0.001f
    };

    [SerializeField]
    private LifecycleEffect radiationExcretion = new() {
        isInfinite = true,
        targetParameterId = RadiationParameterId,
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
