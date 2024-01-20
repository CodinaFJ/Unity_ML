using System;
using System.Collections.Generic;

public class ServiceLocator
    {
        public static ServiceLocator    Instance 
        {
            get => instance ??= new ServiceLocator();
            private set => instance = value;
        }

        private static      ServiceLocator   instance;
        private readonly    Dictionary<Type, object> services;

        public ServiceLocator()
        {
            services = new Dictionary<Type, object>();
        }

        public void RegisterService<T>(T service)
        {      
            var type = typeof(T);

            if (services.ContainsKey(type))
				services.Remove(type);
			services.Add(type, service);
        }

        public void RemoveService<T>(T service)
        {      
            var type = typeof(T);

            if (services.ContainsKey(type))
                services.Remove(type);
        }

        public T GetService<T>()
        {
            var type = typeof(T);

            if (!services.TryGetValue(type, out var service))
                this.LogError($"[ERROR]: Service {type} not found");

            return (T) service;
        }

        public void ClearServices()
        {
            services.Clear();
        }
    }
