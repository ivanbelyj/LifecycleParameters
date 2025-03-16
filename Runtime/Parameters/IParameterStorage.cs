using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParameterStorage
{
    float GetParameterValue(string parameterId);
    void SetParameterValue(string parameterId, float value);
}
