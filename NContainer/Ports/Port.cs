using System;
using NContainer.AdapterProviders;

namespace NContainer.Ports {
    internal class Port<T>:Port
    {
        public void RegisterReflectionAdapter<TA>() where TA:T {
            var item = (AdapterProvider<T>) new ReflectionAdapterProvider<TA>();
            SetAdapter(item);
        }

        public void RegisterInstanceAdapter(T instance) {
            var item = (AdapterProvider<T>) new InstanceAdapterProvider<T>(instance);
            SetAdapter(item);
        }

        public void RegisterFactoryAdapter(Func<Container, T> factory) {
            var item = (AdapterProvider<T>)new FactoryAdapterProvider<T>(factory);
            SetAdapter(item);
        }

        private void SetAdapter(AdapterProvider<T> item) => _currentAdapter = item;

        public AdapterProvider<T> Addapter => _currentAdapter;

        private AdapterProvider<T> _currentAdapter;

        Port<T1> Port.GetTyped<T1>() => this as Port<T1>;
    }

    internal interface Port {
        Port<T> GetTyped<T>();
    }
}