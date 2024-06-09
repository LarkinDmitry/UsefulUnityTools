using System;
using System.Collections.Generic;
using UnityEngine;

namespace KAKuBCE.UsefulUnityTools
{
    public static class LinkLibrary
    {
        private static Dictionary<Type, object> library = new();

        public static void AddLinkTo(object obj)
        {
            if (!library.TryAdd(obj.GetType(), obj))
            {
                Debug.LogError($"the library already contains a reference to an object of type \"{obj.GetType()}\"");
            }
        }

        public static void RemoveLink(object obj)
        {
            library.Remove(obj.GetType());
        }

        public static T GetLinkTo<T>()
        {
            if (library.TryGetValue(typeof(T), out var result))
            {
                return (T)result;
            }
            else
            {
                Debug.LogError($"reference to object of type \"{typeof(T)}\" not found");
                return default;
            }            
        }
        
        public static bool ContainsType(Type type) => library.ContainsKey(type);
    }
}
