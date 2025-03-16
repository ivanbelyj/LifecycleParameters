using System;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Represents a lifecycle parameter. Responsible for calling events
/// when data changes
///</summary>
[Serializable]
internal class LifecycleParameter
{
    private enum ValueState { Min, Intermediate, Max };
    public delegate void ValueChangedHandler(float oldValue, float newValue);

    #region Events
    public event UnityAction MinReached;
    public event UnityAction MaxReached;
    public event UnityAction Recovered;
    public event UnityAction NotRecovered;

    public event ValueChangedHandler ValueChanged;

    /// <summary>
    /// An event called when value to which the restoring effects strive is changed.
    /// For example, a player got sick and his maximum health decreased
    /// </summary>
    public event ValueChangedHandler OnRecoveredValueChanged;
    #endregion

    private string parameterId;
    public string ParameterId {
        get => parameterId;
    }
    
    private ValueState lastValueState;
    private bool isRecovered;

    public float MinValue { get; set; }    
    public float MaxValue { get; set; }

    /// <summary>
    /// The value that the restorative effects will strive for
    /// </summary>
    [SerializeField]
    private float recoveredValue = 1;
    public float RecoveredValue {
        get => recoveredValue;
        set {
            float newRecoveredValue = Mathf.Clamp(value, MinValue, MaxValue);
            float oldRecoveredValue = recoveredValue;

            if (newRecoveredValue != oldRecoveredValue) {
                recoveredValue = newRecoveredValue;
                OnRecoveredValueChanged?.Invoke(oldRecoveredValue, newRecoveredValue);
                HandleRecoveredValueChanged(oldRecoveredValue, newRecoveredValue);
            }
        }
    }

    [SerializeField]
    private float value = 1;
    public float Value {
        get => value;
        set {
            float oldValue = this.value;
            this.value = Mathf.Clamp(value, MinValue, MaxValue);
            if (oldValue != this.value) {
                ValueChanged?.Invoke(oldValue, this.value);
            }
            else {
                return;
            }

            InvokeValueChangedEvents();
        }
    }

    public LifecycleParameter(LifecycleParameterData data)
    {
        MinValue = data.MinValue;
        MaxValue = data.MaxValue;
        RecoveredValue = data.RecoveredValue;
        value = data.InitialValue;
        parameterId = data.ParameterId;
    }

    private void InvokeValueChangedEvents() {
        // If the minimum is reached, and before that the value was different
        if (this.value == MinValue && lastValueState != ValueState.Min) {
            lastValueState = ValueState.Min;
            MinReached?.Invoke();
        }
        if (this.value == MaxValue && lastValueState != ValueState.Max) {
            lastValueState = ValueState.Max;
            MaxReached?.Invoke();
        }
        if (this.value > MinValue && this.value < MaxValue
            && lastValueState != ValueState.Intermediate) {
            lastValueState = ValueState.Intermediate;
            // An event for an intermediate value doesn't make much sense
        }

        if (this.value == RecoveredValue && !isRecovered) {
            isRecovered = true;
            Recovered?.Invoke();
        }
        if (this.value != RecoveredValue && isRecovered) {
            isRecovered = false;
            NotRecovered?.Invoke();
        }
    }

    private void HandleRecoveredValueChanged(float oldRecoveredValue, float newRecoveredValue)  {
        // Ситуация 1: норма была 100% здоровья. Игрок заболел, теперь никакие аптечки
        // не восстановят выше 80%.
        // Если у игрока до болезни было 100% (parameter == initial), то
        // болезнь снижает лишь максимум, а не текущее значение.
        // Если до болезни было 20%, опять же, в этом плане ничего не изменилось
        // Однако, если смена initial попала прямо в текущий параметр,
        // например, игрок заболел, initial стало 80%, а у него и так было 80%,
        // нужно запустить OnInitial (например, остановить эффекты, которые 
        // заканчиваются по наступлении initial)
        
        if (oldRecoveredValue > newRecoveredValue) {
            if (newRecoveredValue == value) {
                isRecovered = true;
                Recovered?.Invoke();
            }
        }

        // Ситуация 2: Игрок вылечился, норма 80% поднялась до нормы 100%
        if (oldRecoveredValue < newRecoveredValue) {
            // Если у игрока до болезни было 80% (parameter == initial),
            // то теперь нужно дать NotInitial, чтобы, например, запустить регенерацию
            if (value < newRecoveredValue) {
                isRecovered = false;
                NotRecovered?.Invoke();
            }
            // Если до выздоровления было 20%, то в этом плане без разницы,
            // является ли максимум 80%, или же 100%

            // Если лечение подняло норму до 100%, а оказалось, что
            // у игрока было выше нормы (not initial), теперь
            // значение совершенно нормально. Например, можно остановить эффекты возврата
            if (value == newRecoveredValue) {
                isRecovered = true;
                Recovered?.Invoke();
                // Строчки повторяются со случаем выше для ясности рассуждений
            }
        }

        // oldInitial == newInitial не бывает по логике
    }
}
