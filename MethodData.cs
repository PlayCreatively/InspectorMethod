using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Freyr.EditorMethod
{
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

        public static Type[] StringsToTypes(string[] type) 
            => type.Select(name => Type.GetType(name)).ToArray();
    }
}

