using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParameterStorage
{
    float GetParameterValue(uint parameterId);
    void SetParameterValue(uint parameterId, float value);
}
