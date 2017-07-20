using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace NContainer {
    internal class Port<T>:Port
    {
        private readonly ICollection<AdapterProvider<T>> adapters = new List<AdapterProvider<T>>();

        public Port<T> RegisterReflectionAdapter<TA>() where TA:T {
            var item = (AdapterProvider<T>) new ReflectionAdapterProvider<TA>();
            adapters.Add(item);
            return this;
        }

        public Port<T> RegisterInstanceAdapter(T instance) {
            var item = (AdapterProvider<T>) new InstanceAdapterProvider<T>(instance);
            adapters.Add(item);
            return this;
        }

        public Port<T> RegisterFactoryAdapter(Func<Container, T> factory) {
            var item = (AdapterProvider<T>) new FactoryAdapterProvider<T>(factory);
            adapters.Add(item);
            return this;
        }

        public AdapterProvider<T> GetDefaultAddapter() => adapters.Last();
    }

    internal class Port {
        public Port<T> GetTyped<T>() {
            return (Port<T>) this;
        }
    }
}