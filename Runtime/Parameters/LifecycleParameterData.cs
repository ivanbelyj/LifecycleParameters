using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LifecycleParameterData
{
    [SerializeField]
    private uint parameterId = 0;
    public uint ParameterId { get => parameterId; set => parameterId = value; }

    [SerializeField]
    private float minValue = 0;
    public float MinValue { get => minValue; set => minValue = value; }

    [SerializeField]
    private float maxValue = 1;
    public float MaxValue { get => maxValue; set => maxValue = value; }

    [SerializeField]
    private float recoveredValue = 1;
    public float RecoveredValue { get => recoveredValue; set => recoveredValue = value; }

    [SerializeField]
    private float initialValue = 1;
    public float InitialValue { get => initialValue; set => initialValue = value; }
}
