using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LifecycleParameterData
{
    [SerializeField]
    private uint parameterId = 0;
    public uint ParameterId { get => parameterId; }

    [SerializeField]
    private float minValue = 0;
    public float MinValue { get => minValue; }

    [SerializeField]
    private float maxValue = 1;
    public float MaxValue { get => maxValue; }

    [SerializeField]
    private float recoveredValue = 1;
    public float RecoveredValue { get => recoveredValue; }

    [SerializeField]
    private float initialValue = 1;
    public float InitialValue { get => initialValue; }
}
