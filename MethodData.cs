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
        [SerializeField] public object targetInstance;
        //[SerializeField] protected string[] parameters;
        [SerializeField] protected int selectedIndex;
        [SerializeField] protected string targetType;

        public bool IsInstance => isInstance;
        [SerializeField] bool isInstance;

        //Nonserializable Cash//
        protected Delegate method;
        Type GetTargetType => targetTypeCash ?? (targetTypeCash = Type.GetType(targetType));
        Type targetTypeCash;
        public MethodInfo GetMethodInfo => 
            methodInfoCash ?? (methodInfoCash = (method != null) ? method.Method : GetTargetType.GetMethod(methodName, GetAppropriateBindings()));

        MethodInfo methodInfoCash;

        protected void AssignTargetInfo(object target)
        {
            if (target != null)
            {
                targetInstance = target;
                targetTypeCash = target.GetType();
                targetType = targetTypeCash.Name;
                isInstance = true;
            }
            else throw new Exception("The provided target is null");
        }
        protected void AssignTargetType(Type targetType)
        {
            targetTypeCash = targetType;
            this.targetType = targetType.Name;
        }
        protected void AssignMethodName(MethodInfo methodInfo)
        {
            methodName = methodInfo.Name;
            methodInfoCash = methodInfo;
        }

        BindingFlags GetAppropriateBindings()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;
            if (IsInstance) bindingFlags |= BindingFlags.Instance;
            return bindingFlags;
        }

        public MethodData(Delegate method)
        {
            this.method = method;
            AssignMethodName(method.Method);
            if (method.Target != null)
                AssignTargetInfo(method.Target);
            else
                AssignTargetType(methodInfoCash.DeclaringType);
        }

        public MethodData(string methodName, object target)
        {
            this.methodName = methodName;
            AssignTargetInfo(target);
        }

        public MethodData(string methodName, Type targetType)
        {
            this.methodName = methodName;
            AssignTargetType(targetType);
        }

        public MethodData(MethodInfo methodInfo)
        {
            AssignMethodName(methodInfo);
            AssignTargetType(methodInfo.DeclaringType);
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
            if(IsInstance)
            {
                if (targetInstance != null)
                    return Delegate.CreateDelegate(typeof(T), targetInstance, methodName) as T;
                else
                    throw new Exception($"Can't bind since {nameof(targetInstance)} is null. Try reassigning the target.");
            }
                
            try {
                return Delegate.CreateDelegate(typeof(T), GetTargetType, methodName) as T;
            }
            catch (Exception e) {
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

