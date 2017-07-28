using System.Diagnostics;

namespace NContainer.AdapterProviders {
#if !DEBUG

    [DebuggerStepThrough]
#endif
    internal class InstanceAdapterProvider<T> : AdapterProvider<T> {
        private readonly T _instance;

        public InstanceAdapterProvider(T instance) {
            _instance = instance;
        }

        public T GrabInstance(Container container) {
            return _instance;
        }
    }
}