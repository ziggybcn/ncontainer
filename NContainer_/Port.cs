using System;

namespace NContainer {
    internal class Port<T>:Port
    {
        public Port<T> RegisterReflectionAdapter<TA>() where TA:T {
            var item = (AdapterProvider<T>) new ReflectionAdapterProvider<TA>();
            SetAdapter(item);
            return this;
        }

        public Port<T> RegisterInstanceAdapter(T instance) {
            var item = (AdapterProvider<T>) new InstanceAdapterProvider<T>(instance);
            SetAdapter(item);
            return this;
        }

        public Port<T> RegisterFactoryAdapter(Func<Container, T> factory) {
            var item = (AdapterProvider<T>)new FactoryAdapterProvider<T>(factory);
            SetAdapter(item);
            return this;
        }

        private void SetAdapter(AdapterProvider<T> item) => _currentAdapter = item;

        public AdapterProvider<T> Addapter => _currentAdapter;

        private AdapterProvider<T> _currentAdapter;

    }

    internal class Port {
        public Port<T> GetTyped<T>() {
            return (Port<T>) this;
        }
    }
}