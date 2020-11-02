using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using TypeReferences;
using UnityEngine;

[Serializable]
public class MethodData
{
    [SerializeField] protected string methodName;
    [SerializeField] protected UnityEngine.Object targetInstance;
    //[SerializeField] string delegateType;
    [SerializeField] protected string[] parameters;
    [SerializeField] protected int selectedIndex;
    [SerializeField] protected string targetType;

    public MethodInfo GetMethodInfo() => Type.GetType(targetType).GetMethod(methodName);

    public T ToDelegate<T>() where T : Delegate
    {
        return Delegate.CreateDelegate(typeof(T), targetInstance, methodName) as T;
    }

    public Delegate ToDelegate()
    {
        Type type = GetMatchingDelegate(parameters.Length);
        Debug.LogError(parameters.Length);

        return Delegate.CreateDelegate(type, typeof(SerialTest), methodName);
    }


    public static Type[] StringsToTypes(string[] type)
    {
        return type.Select(name => Type.GetType(name)).ToArray();
    }


    public static Type GetMatchingDelegate(int parameterCount)
    {
        switch (parameterCount)
        {
            case 0: return typeof(Action);
            case 1: return typeof(Action<dynamic>);
            case 2: return typeof(Action<dynamic, dynamic>);
            case 3: return typeof(Action<,,>);
            case 4: return typeof(Action<,,,>);
            default: return null;
        }
    }

}
