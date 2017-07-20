using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pico {
    internal class Port<T>:Port
    {
        private readonly ICollection<Adapter<T>> adapters = new List<Adapter<T>>();

        public Port<T> RegisterAddapter<TA>() where TA:T {
            Adapter<T> item = (Adapter<T>) new ReflectionAdapter<TA>();
            adapters.Add(item);
            return this;
        }

        public Adapter<T> GetDefaultAddapter() => adapters.First();
    }

    class Port {
        public Port<T> GetTyped<T>() {
            return (Port<T>) this;
        }
    }

    internal class ReflectionAdapter<T> : Adapter<T>
    {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {

            if (Constructors.Length == 0) throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var parameters = Constructors[0].GetParameters();
            var myParams = parameters.Select(parameter => container.GetInstance(parameter.ParameterType));

            return (T)Constructors[0].Invoke( myParams.ToArray());

        }
    }


    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) { }
    }

    interface Adapter<out T>
    {
        T GrabInstance(Container container);
    }

}