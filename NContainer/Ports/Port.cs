using System;
using NContainer.AdapterProviders;

namespace NContainer.Ports {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class Port<T> : Port {
        public AdapterProvider<T> Addapter { get; private set; }

        Port<TI> Port.GetTyped<TI>() => this as Port<TI>;

        internal void RegisterReflectionAdapter<TA>() where TA : T => 
            SetAdapter((AdapterProvider<T>)new ReflectionAdapterProvider<TA>());

        internal void RegisterInstanceAdapter(T instance) => 
            SetAdapter(new InstanceAdapterProvider<T>(instance));

        internal void RegisterFactoryAdapter(Func<Container, T> factory) => 
            SetAdapter(new FactoryAdapterProvider<T>(factory));

        internal void RegisterLazyFactoryAdapter(Container container, Func<Container, T> lazyFactory) => 
            SetAdapter(new LazyAdapterProvider<T>(container, lazyFactory));

        internal void RegisterDeferredSingleton<TA>(Container container) where TA : T => 
            SetAdapter((AdapterProvider<T>)new DeferredSingleton<TA>(container));

        internal void SetAdapter(AdapterProvider<T> item) => Addapter = item;
    }

    internal interface Port {
        Port<TI> GetTyped<TI>();
    }
}