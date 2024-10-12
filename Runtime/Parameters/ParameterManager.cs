using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

internal class ParameterManager
{
    private readonly IParameterStorage parameterStorage;

    ///<summary>
    /// All lifecycle parameters collected for traversal.
    /// Dynamic addition / removal of parameters is not assumed
    ///</summary>
    private IReadOnlyDictionary<uint, LifecycleParameter> Parameters { get; set; }

    public ParameterManager(
        IEnumerable<LifecycleParameter> initialParameters,
        IParameterStorage parameterStorage)
    {
        this.parameterStorage = parameterStorage;
        Parameters = initialParameters.ToDictionary(x => x.ParameterId, x => x);

        InitializeParameterStorage(initialParameters);
    }

    private void InitializeParameterStorage(IEnumerable<LifecycleParameter> initialParameters) {
        foreach (var initialParameter in initialParameters) {
            parameterStorage.SetParameterValue(
                initialParameter.ParameterId,
                initialParameter.Value);
        }

        AddParameterValueChangeHandlers(initialParameters);
    }

    public LifecycleParameter GetParameter(uint parameterId) {
        return Parameters[parameterId];
    }

    public void ApplyEffect(LifecycleEffect effect) {
        // If the effect is infinite or has not completed
        if (effect.isInfinite || !effect.IsPassed) {
            LifecycleParameter target = Parameters[effect.targetParameterId];
            target.Value += effect.speed * Time.deltaTime;
        }
    }

    private void AddParameterValueChangeHandlers(
        IEnumerable<LifecycleParameter> initialParameters) {
        foreach (var parameter in initialParameters) {
            parameter.ValueChanged += (oldValue, newValue) => {
                OnParameterValueChanged(parameter.ParameterId, oldValue, newValue);
            };
        }
    }

    private void OnParameterValueChanged(
        uint parameterId,
        float oldValue,
        float newValue) {
        parameterStorage.SetParameterValue(parameterId, newValue);
    }
}
