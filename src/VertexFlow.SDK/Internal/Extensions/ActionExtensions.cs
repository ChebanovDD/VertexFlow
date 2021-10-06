using System;

namespace VertexFlow.SDK.Internal.Extensions
{
    internal static class ActionExtensions
    {
        public static T Invoke<T>(this Action<T> action) where T : class, new()
        {
            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            var instance = new T();
            action.Invoke(instance);
            return instance;
        }
    }
}