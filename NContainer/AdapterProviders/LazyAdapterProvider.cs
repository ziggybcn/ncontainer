using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NContainer.AdapterProviders
{
    internal class LazyAdapterProvider<T> : AdapterProvider<T>
    {        
        private readonly Lazy<T> _lazyInstance;

        public LazyAdapterProvider(Container container,Func<Container, T> factoryMethod)
        {
            _lazyInstance = new Lazy<T>(()=>factoryMethod.Invoke(container));
        }

        public T GrabInstance(Container container) => _lazyInstance.Value;
    }
}

