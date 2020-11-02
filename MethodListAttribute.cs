using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MethodListAttribute : PropertyAttribute
{
    public BindingFlags BindingFlags { get; set; }
    public Type TargetType { get; set; }
}
