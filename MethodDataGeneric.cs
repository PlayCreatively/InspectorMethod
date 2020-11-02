using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MethodData<O> : MethodData
{
    public O Invoke()
    {
        object output = GetMethodInfo().Invoke(targetInstance, null);
        if (output is O match) return match;
        else return (O)output;
    }
}

[Serializable]
public class MethodData<O,I1> : MethodData
{
    public O Invoke(I1 value)
    {
        object output = GetMethodInfo().Invoke(targetInstance, new object[] { value }, true);
        if (output is O match) return match;
        else return (O)output;
    }
}

[Serializable]
public class MethodData<O, I1, I2> : MethodData
{
    public O Invoke(I1 value1, I2 value2)
    {
        object output = GetMethodInfo().Invoke(targetInstance, new object[] { value1, value2 }, true);
        if (output is O match) return match;
        else return (O)output;
    }
}

[Serializable]
public class MethodData<O, I1, I2, I3> : MethodData
{
    public O Invoke(I1 value1, I2 value2, I3 value3)
    {
        object output = GetMethodInfo().Invoke(targetInstance, new object[] { value1, value2, value3 }, true);
        if (output is O match) return match;
        else return (O) output;
    }
}