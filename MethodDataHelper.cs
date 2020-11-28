using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Freyr.EditorMethod
{
    public static class MethodDataHelper
    {
        public static IEnumerable<MethodInfo> GetMethodsExtension(this Type type, BindingFlags flags, params Type[] paramTypes)
        {
            return type.GetMethods(flags)
                .Select(m => new { m, Params = m.GetParameters() })
                .Where(x =>
                {
                    if (paramTypes == null) return x.Params.Length == 0;
                    else if (paramTypes.Length == 0) return true;
                    else return x.Params.Length == paramTypes.Length &&
                     x.Params.Select(p => p.ParameterType).ToArray().IsEqualTo(paramTypes);
                })
                .Select(x => x.m);
        }

        public static string ToOneLine(this IEnumerable<string> stringEnum, string between, string forLast)
        {
            int count = stringEnum.Count();
            string outputString = string.Empty;
            if (count == 0) return outputString;

            IEnumerator<string> enumerator = stringEnum.GetEnumerator();

            enumerator.MoveNext();
            outputString += $"{enumerator.Current}";
            if (count == 1) return outputString;

            for (int i = 0; i < count-2; i++)
            {
                enumerator.MoveNext();
                outputString += $"{between} {enumerator.Current}";
            }

            enumerator.MoveNext();
            outputString += $" {forLast} {enumerator.Current}";



            return outputString;
        }

        public static bool IsEqualTo<T>(this IList<T> list, IList<T> other)
        {
            if (list.Count != other.Count) return false;
            for (int i = 0, count = list.Count; i < count; i++)
                if (!list[i].Equals(other[i]))
                    return false;
            return true;
        }

        /// <summary>
        /// Checks if parameter types match and if offered parameters reach required amount.
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool AreParametersEqual(ParameterInfo[] parameterInfos, Type[] types, bool exactMatch)
        {
            if (exactMatch && parameterInfos.Length != types.Length) return false;
            else if (parameterInfos.Length > types.Length) return false;

            bool results = true;
            for (int i = 0; i < parameterInfos.Length; i++)
                results &= parameterInfos[i].ParameterType == types[i];
            return results;
        }

        public static string ToReadableString(this ParameterInfo parameterInfo) => $"{parameterInfo.ParameterType.Name} {parameterInfo.Name}";
        public static string ToReadableString(this ParameterInfo[] parameterInfo)
        {
            if (parameterInfo == null || parameterInfo.Length == 0) return string.Empty;

            string print = parameterInfo[0].ToReadableString();
            for (int i = 1; i < parameterInfo.Length; i++)
                print += ", " + parameterInfo[i].ToReadableString();
            return print;
        }
        public static string ToReadableString(this MethodInfo methodInfo)
        {
            string readableText = $"{methodInfo.ReturnType.Name} ";
            if (methodInfo.Name.Contains("get_"))
                readableText += methodInfo.Name.Substring("get_".Length);
            else
                readableText +=  $"{methodInfo.Name} ({methodInfo.GetParameters().ToReadableString()})";
            return readableText;
        }

        public static object Invoke(this MethodBase methodBase, object target, object[] parameters, bool ignoreParamCount)
        {
            if (ignoreParamCount)
            {
                int count = methodBase.GetParameters().Length;
                if (count < parameters.Length) 
                    Array.Resize(ref parameters, count);
                else if (count > parameters.Length)
                    throw new TargetParameterCountException($"Parameters don't meet required amount. Expected {count} but got {parameters.Length}.");
            }
        
            return methodBase.Invoke(target, parameters);
        }

        public static MethodData<TOut> GetMethodData<TOut>(this Func<TOut> method) => method;
        public static MethodData<TIn, TOut> GetMethodData<TIn, TOut>(this Func<TIn, TOut> method) => method;
        public static MethodData<TIn1, TIn2, TOut> GetMethodData<TIn1, TIn2, TOut>(this Func<TIn1, TIn2, TOut> method) => method;
    }
}
