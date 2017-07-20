using System;

namespace NContainer {
    public class FactoryAdapter<T> : Adapter<T> {
        private readonly Func<Container, T> _factory;
        public FactoryAdapter(Func<Container, T> factory) {
            _factory = factory;
        }

        public T GrabInstance(Container container) => _factory(container);
    }
}