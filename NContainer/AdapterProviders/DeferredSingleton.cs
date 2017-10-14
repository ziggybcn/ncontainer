using System;

namespace NContainer.AdapterProviders {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class DeferredSingleton<T> : AdapterProvider<T>, ReflectionConstruction<T> {

        private readonly Lazy<T> _lazyInstance;

        public T GrabInstance(Container container) => _lazyInstance.Value;

        internal DeferredSingleton(Container container) => 
            _lazyInstance = new Lazy<T>(() => Reflector.GrabInstance(container));

         public ReflectionSolver<T> Reflector { get; } = new ReflectionSolver<T>();

    }
}