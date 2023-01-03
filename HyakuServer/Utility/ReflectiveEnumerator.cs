using System;
using System.Collections.Generic;
using System.Linq;

namespace HyakuServer.Utility
{
    public class ReflectiveEnumerator
    {
        public static IEnumerable<T> GetEnumerableOfType<T>() where T : class
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())         
                .Where(type => type.IsSubclassOf(typeof(T)))
                .Select(type => Activator.CreateInstance(type) as T);
        }
    }
}