using System;
using System.Reflection;
using UnityEngine;

namespace Freyr.EditorMethod
{
    public class MethodListAttribute : PropertyAttribute
    {
        public BindingFlags BindingFlags { get; set; } = BindingFlags.Public | BindingFlags.Static;

        /// <summary>Find methods that match exactly. If set to false you can no longer assume its delegate type and must invoke using reflection.</summary>
        public bool ExactMatch { get; set; } = true;
        public Type TargetType { get; set; } = typeof(object);
    }
}
