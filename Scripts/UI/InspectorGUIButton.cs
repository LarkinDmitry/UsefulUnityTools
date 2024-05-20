using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace KAKuBCE.UsefulUnityTools
{
    /// <summary>
    /// Draw button in Inspector
    /// </summary>
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class InspectorGUIButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var target in targets)
            {
                var mis = target.GetType().GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == typeof(EditorButtonAttribute)));
                if (mis != null)
                {
                    foreach (var mi in mis)
                    {
                        if (mi != null)
                        {
                            var attribute = (EditorButtonAttribute)mi.GetCustomAttribute(typeof(EditorButtonAttribute));
                            if (GUILayout.Button(attribute.name))
                            {
                                mi.Invoke(target, null);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Attribute from method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorButtonAttribute : Attribute
    {
        /// <summary>
        /// Button text
        /// </summary>
        public string name;

        /// <summary>
        /// Add Button to Inspector
        /// </summary>
        /// <param name="name">Button text</param>
        public EditorButtonAttribute(string name)
        {
            this.name = name;
        }
    }

    /*
    [EditorButton("Example text on button")]
    public void ExampleMethod()
    {
        Debug.Log("Example button click!");
    }
    */
}