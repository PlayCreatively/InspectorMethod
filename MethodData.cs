using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace Freyr.EditorMethod
{
    [Serializable]
    public class MethodData
    {
        //Serializable//
        [SerializeField] protected string methodName;
        [SerializeField] protected UnityEngine.Object targetInstance;
        //[SerializeField] protected string[] parameters;
        [SerializeField] protected int selectedIndex;
        [SerializeField] protected string targetType;

        //Nonserializable Cash//
        Type GetTargetType => targetTypeHash ?? (targetTypeHash = Type.GetType(targetType));
        Type targetTypeHash;
        public MethodInfo GetMethodInfo => methodInfoHash ?? (methodInfoHash = GetTargetType.GetMethod(methodName, GetAppropriateBindings()));
        MethodInfo methodInfoHash;

        BindingFlags GetAppropriateBindings()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;
            if (targetInstance) bindingFlags |= BindingFlags.Instance;
            return bindingFlags;
        }

        public MethodData(string methodName, Type targetType)
        {
            this.methodName = methodName;
            this.targetType = targetType.Name;
            targetTypeHash = targetType;
        }

        public MethodData(string methodName, UnityEngine.Object target) : this(methodName, target.GetType())
        {
            targetInstance = target;
        }

        public MethodData(MethodInfo methodInfo)
        {
            methodInfoHash = methodInfo;
            methodName = methodInfo.Name;
            targetType = methodInfo.DeclaringType.Name;
            targetTypeHash = methodInfo.DeclaringType;
        }

        /// <summary>Invokes using reflection.</summary>
        public object Invoke(params object[] parameters) 
            => GetMethodInfo.Invoke(targetInstance, parameters);

        /// <summary>Invokes using reflection.</summary>
        /// <param name="ignoreParamCount">Shorten parameter array to fit invoking method. Slower to ignoreParamCount.</param>
        public object Invoke(bool ignoreParamCount, params object[] parameters)
            => GetMethodInfo.Invoke(targetInstance, parameters, ignoreParamCount);

        public T ToDelegate<T>() where T : Delegate
        {
            try
            {
                if(targetInstance != null) 
                    return Delegate.CreateDelegate(typeof(T), targetInstance, methodName) as T;
                else
                    return Delegate.CreateDelegate(typeof(T), GetTargetType, methodName) as T;
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message} If your intention is to invoke the method try using the equivalent overload that uses reflections.");
            }
        }
        public bool ToDelegate<T>(out T outDelegate) where T : Delegate
        {
            outDelegate = ToDelegate<T>();
            return outDelegate != null;
        }

        public static Type[] StringsToTypes(string[] type) 
            => type.Select(name => Type.GetType(name)).ToArray();
    }
}

