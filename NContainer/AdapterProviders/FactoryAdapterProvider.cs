using System;

namespace NContainer.AdapterProviders {
    internal class FactoryAdapterProvider<T> : AdapterProvider<T> {
        private readonly Func<Container, T> _factory;

        public FactoryAdapterProvider(Func<Container, T> factory) {
            _factory = factory;
        }

        public T GrabInstance(Container container) => _factory.Invoke(container);
    }
}