using System;
using System.Reflection;
using UnityEngine;

namespace Freyr.EditorMethod
{
    public class MethodListAttribute : PropertyAttribute
    {
        public BindingFlags BindingFlags { get; set; }
        public Type TargetType { get; set; }
    }
}
