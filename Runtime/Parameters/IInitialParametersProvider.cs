using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitialParametersProvider
{
    LifecycleParameterData[] GetInitialParameters();
}
