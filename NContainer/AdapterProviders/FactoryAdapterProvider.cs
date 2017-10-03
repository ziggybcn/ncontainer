using System;
using System.Diagnostics;

namespace NContainer.AdapterProviders {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class FactoryAdapterProvider<T> : AdapterProvider<T> {
        private readonly Func<Container, T> _factory;

        public FactoryAdapterProvider(Func<Container, T> factory) {
            _factory = factory;
        }

        public T GrabInstance(Container container) => _factory(container);
    }
}