using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class FreyrDelegateBase<T>
{
    [MethodList(TargetType = typeof(BoardManager))]
    public MethodData<bool, T> Condition;
    [MethodList(TargetType = typeof(SerialTest))]
    public MethodData<object, T> OnSuccess;

    public bool Try(T value)
    {
        bool successful = Condition.Invoke(value);
        if (successful)
            OnSuccess.Invoke(value);
        return successful;
    }
}