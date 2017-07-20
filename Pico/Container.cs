using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;

namespace NContainer {
    public class Container {

        #region Register ports and adapters
        /// <summary>
        /// Pairs an interface with a speciffic class.
        /// </summary>
        /// <typeparam name="P">The interface</typeparam>
        /// <typeparam name="A">The actual class that implements the interface</typeparam>
        public Container Register<P, A>() where A : P {
            GetPortForAGivenContract<P>().RegisterReflectionAdapter<A>();
            return this;
        }

        /// <summary>
        /// Paris a given class with each one of its interfaces
        /// </summary>
        /// <typeparam name="A">The class to be registered</typeparam>
        public Container Register<A>() {
            foreach (var myType in typeof(A).GetInterfaces()) {
                var registrationMethod = RegisterPortAdapterMethod.MakeGenericMethod(myType, typeof(A));
                registrationMethod.Invoke(this, null);
            }
            return this;
        }

        /// <summary>
        /// Pairs interface with a static instance of a class (useful for singleton-like needs)
        /// </summary>
        /// <typeparam name="P">the interface</typeparam>
        /// <param name="instance">the class intance implementing the interface</param>
        public Container Register<P>(P instance) {
            GetPortForAGivenContract<P>().RegisterInstanceAdapter(instance);
            return this;
        }

        /// <summary>
        /// Pairs an interface with a factory method
        /// </summary>
        /// <typeparam name="P">The interface</typeparam>
        /// <param name="factory">The factory method</param>
        public Container Register<P>(Func<Container, P> factory) {
            GetPortForAGivenContract<P>().RegisterFactoryAdapter(factory);
            return this;
        }
        #endregion

        #region obtain adapters for given ports
        /// <summary>
        /// Returns an instance of a registered class. Notice the generic version of this method is prefered always.
        /// </summary>
        /// <param name="contract">The interface</param>
        public object GetInstance(Type contract) {
            var genericMethod = GetInstanceMethod.MakeGenericMethod(contract);
            return genericMethod.Invoke(this, null);
        }


        /// <summary>
        /// Returns an instance of a registered class
        /// </summary>
        /// <typeparam name="T">The interface</typeparam>
        public T GetInstance<T>() {
            var myType = typeof(T);
            AdapterProvider<T> adapter;
            try {
                var port = ports[myType].GetTyped<T>();
                adapter = port.GetDefaultAddapter();
            }
            catch (KeyNotFoundException e) {
                throw new UnresolvedInterfaceException(myType);
            }
            
            return adapter.GrabInstance(this);
        }
        #endregion


        #region Private stuff and implementation detail
        private Port<TP> GetPortForAGivenContract<TP>() {
            Port adapterProvider;
            if (ports.TryGetValue(typeof(TP), out adapterProvider)) return (Port<TP>)adapterProvider;

            var port = new Port<TP>();
            ports.Add(typeof(TP), port);
            return port;
        }

        private Dictionary<Type, Port> ports = new Dictionary<Type, Port>();

        private static readonly MethodInfo GetInstanceMethod =
                    typeof(Container).GetMethods().First(m =>
                m.Name == "GetInstance" && m.IsGenericMethod && m.GetGenericArguments().Length == 1);

        private static readonly MethodInfo RegisterPortAdapterMethod =
                    typeof(Container).GetMethods().First(m =>
                m.Name == "Register" && m.IsGenericMethod && m.GetGenericArguments().Length == 2);

        #endregion

    }

    public class UnresolvedInterfaceException : Exception {
        internal UnresolvedInterfaceException(Type dependency):base($"No class provider was found for the {dependency.Name} interface") { }
    }

}