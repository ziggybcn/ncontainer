using System;

namespace NContainer.AdapterProviders
{
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class LazyAdapterProvider<T> : AdapterProvider<T>
    {        
        private readonly Lazy<T> _lazyInstance;

        public LazyAdapterProvider(Container container,Func<Container, T> factoryMethod)
        {
            _lazyInstance = new Lazy<T>( ()=> factoryMethod.Invoke(container));
        }

        public T GrabInstance(Container container) => _lazyInstance.Value;
    }
}

