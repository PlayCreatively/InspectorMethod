using System;
using System.Reflection;
using UnityEngine;

namespace Freyr.EditorMethod
{
    public class MethodListAttribute : PropertyAttribute
    {
        public BindingFlags BindingFlags { get; set; } = BindingFlags.Public | BindingFlags.Static;
        public bool ExactMatch { get; set; } = true;
        public Type TargetType { get; set; } = typeof(object);
    }
}
