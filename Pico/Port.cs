using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Pico {
    public class Port<T>:Port
    {
        private readonly ICollection<Adapter<T>> adapters = new List<Adapter<T>>();

        public Port<T> RegisterAddapter<TA>() where TA:T {
            Adapter<T> item = (Adapter<T>) new ReflectionAdapter<TA>();
            adapters.Add(item);
            return this;
        }

       // public Port<T> RegisterAdapter

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