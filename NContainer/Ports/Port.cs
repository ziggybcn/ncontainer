using System;
using System.Diagnostics;
using NContainer.AdapterProviders;

namespace NContainer.Ports {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class Port<T> : Port {
        public AdapterProvider<T> Addapter { get; private set; }

        Port<TI> Port.GetTyped<TI>() {
            return this as Port<TI>;
        }

        public void RegisterReflectionAdapter<TA>() where TA : T {
            var item = (AdapterProvider<T>) new ReflectionAdapterProvider<TA>();
            SetAdapter(item);
        }

        public void RegisterInstanceAdapter(T instance) {
            var item = (AdapterProvider<T>) new InstanceAdapterProvider<T>(instance);
            SetAdapter(item);
        }

        public void RegisterFactoryAdapter(Func<Container, T> factory) {
            var item = (AdapterProvider<T>) new FactoryAdapterProvider<T>(factory);
            SetAdapter(item);
        }

        public void RegisterLazyAdapter(Container container, Func<Container, T> lazyFactory) {
            var item = (AdapterProvider<T>)new LazyAdapterProvider<T>(container,lazyFactory);
            SetAdapter(item);

        }

        public void RegisterDeferredSingleton<TA>(Container container) {
            var item = (AdapterProvider<T>)new DeferredSingleton<TA>(container);
            SetAdapter(item);
        }

        private void SetAdapter(AdapterProvider<T> item) {
            Addapter = item;
        }
    }

    internal interface Port {
        Port<TI> GetTyped<TI>();
    }
}