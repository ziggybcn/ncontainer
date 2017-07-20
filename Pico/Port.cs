using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace NContainer {
    public class Port<T>:Port
    {
        private readonly ICollection<Adapter<T>> adapters = new List<Adapter<T>>();

        public Port<T> RegisterAdapter<TA>() where TA:T {
            var item = (Adapter<T>) new ReflectionAdapter<TA>();
            adapters.Add(item);
            return this;
        }

        public Port<T> RegisterAdapter(T instance) {
            var item = (Adapter<T>)new InstanceAdapter<T>(instance);
            adapters.Add(item);
            return this;
        }

        public Port<T> RegisterAdapter(Func<Container, T> factory) {
            var item = (Adapter<T>)new FactoryAdapter<T>(factory);
            adapters.Add(item);
            return this;
        }

        public Adapter<T> GetDefaultAddapter() => adapters.First();
    }

    public class Port {
        public Port<T> GetTyped<T>() {
            return (Port<T>) this;
        }
    }


    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) { }
    }



}