#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Zyntra.Data
{
    [InitializeOnLoad]
    public static class SerializableEnforcer
    {
        static SerializableEnforcer()
        {
            var targetType = typeof(Objects.TimelineObject);
            var types = Assembly.GetAssembly(targetType)
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && targetType.IsAssignableFrom(t));

            foreach (var type in types)
            {
                if (!type.IsDefined(typeof(SerializableAttribute), false))
                {
                    Debug.LogError($"[Zyntra] Class {type.Name} has no serializable attribute!\n" +
                                   $"It is required for data loading to work!");
                }
            }
        }
    }
}
#endif