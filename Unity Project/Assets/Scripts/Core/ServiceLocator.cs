using System;
using System.Collections.Generic;

namespace CardGame.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new ();
        
        public static void Register<T>(T service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service), "Service cannot be null.");

            var type = typeof(T);

            _services.TryAdd(type, service);
        }
        
        public static T Get<T>()
        {
            var type = typeof(T);

            if (!_services.TryGetValue(type, out var service))
                throw new KeyNotFoundException($"Service of type {type.Name} is not registered.");

            return (T)service;
        }
    }
}