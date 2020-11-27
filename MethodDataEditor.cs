using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Freyr.EditorMethod
{
    [CustomPropertyDrawer(typeof(MethodListAttribute))]
    [CustomPropertyDrawer(typeof(MethodData), true)]
    public class MethodDataEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type[] paramTypes = null;
            Type returnType = null;

            if (property.propertyType == SerializedPropertyType.Generic)
            {
                var genericTypeArguments = fieldInfo.FieldType.GenericTypeArguments;

                int length = genericTypeArguments.Length;
                returnType = genericTypeArguments[length-1];
                if(length > 1)
                {
                    Array.Resize(ref genericTypeArguments, length - 1);
                    paramTypes = genericTypeArguments;
                }
            }

            //Properties
            SerializedProperty selectedIndex = property.FindPropertyRelative(nameof(selectedIndex));
            SerializedProperty methodName = property.FindPropertyRelative(nameof(methodName));
            SerializedProperty parameters = property.FindPropertyRelative(nameof(parameters));
            SerializedProperty targetInstance = property.FindPropertyRelative(nameof(targetInstance));
            SerializedProperty targetType = property.FindPropertyRelative(nameof(targetType));

            position.height = 20f;

            BindingFlags bindingFlags = BindingFlags.Default;
            Type targetTypeValue = null;

            if (attribute is MethodListAttribute freyrDelegateAttr)
            {
                bindingFlags = freyrDelegateAttr.BindingFlags;
                targetTypeValue = freyrDelegateAttr.TargetType;
            }

            if(bindingFlags == BindingFlags.Default)
                bindingFlags = BindingFlags.Public | BindingFlags.Static;
            if(targetTypeValue == null)
                targetTypeValue = targetInstance.objectReferenceValue == null ? typeof(object) : targetInstance.objectReferenceValue.GetType();

            //Method array
            MethodInfo[] methodInfos = targetTypeValue.GetMethodsExtension(bindingFlags)
                .Where(method => returnType == typeof(object) || method.ReturnType == returnType)
                .Where(method => MethodDataHelper.AreParametersEqual(method.GetParameters(), paramTypes))
                .ToArray();

            //if no methods are found
            if (methodInfos == null || methodInfos.Length == 0)
            {
                position.height += 25f;
                EditorStyles.helpBox.richText = true;
                EditorGUI.HelpBox(position, $"No Methods found in <b>{targetTypeValue}</b> that take parameters of type <b>{paramTypes.Select(type => type.Name).ToOneLine(",", "and")}</b>", MessageType.Warning);
                return;
            }
            string[] methodList = methodInfos.Select(method => method.ToReadableString()).ToArray();

            //If selected index is out of bounds
            if (methodList.Length < selectedIndex.intValue) selectedIndex.intValue = 0;
            EditorStyles.popup.richText = true;
            selectedIndex.intValue = EditorGUI.Popup(position, property.displayName, selectedIndex.intValue, methodList);
            //selectedIndex property
            methodName.stringValue = methodInfos[selectedIndex.intValue].Name;

            //parameters property
            var paramNames = methodInfos[selectedIndex.intValue].GetParameters().Select(param => param.ParameterType.Name).ToArray();
            int sizeDif = -parameters.arraySize + paramNames.Length;
            if (sizeDif < 0)
                for (; sizeDif != 0; sizeDif++)
                    parameters.DeleteArrayElementAtIndex(0);
                    
            else if (sizeDif > 0)
                for (;-sizeDif != 0; sizeDif--)
                    parameters.InsertArrayElementAtIndex(0);

            for (int i = 0; i < paramNames.Length; i++)
                parameters.GetArrayElementAtIndex(i).stringValue = paramNames[i];

            //Target Type
            targetType.stringValue = targetTypeValue.ToString();


            //Target instance property
            if (bindingFlags.HasFlag(BindingFlags.Instance))
            {
                position.y += 20f;
                position.height = 20f;
                targetInstance.objectReferenceValue = EditorGUI.ObjectField(position, targetInstance.objectReferenceValue, typeof(object), true);
            }

            position.y += 10f;

            if (GUI.changed)
                EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 1;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }
    }
}

