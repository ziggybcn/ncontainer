using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NContainer.AdapterProviders;
using NContainer.Ports;

namespace NContainer {
    public class Container {
        public Container() { }

        /// <summary>
        /// Use this constructor to import data from other container upon container creation.
        /// </summary>
        /// <param name="containerToImport"></param>
        public Container(Container containerToImport) {
            ImportContainer(containerToImport);
        }

        #region Register ports and adapters

        /// <summary>
        /// Pairs an interface with a speciffic class.
        /// </summary>
        /// <typeparam name="TP">The interface</typeparam>
        /// <typeparam name="TA">The actual class that implements the interface</typeparam>
        // ReSharper disable once UnusedMethodReturnValue.Global (allows for fluent syntax)
        public Container Register<TP, TA>() where TA : TP {
            GetPortForAGivenContract<TP>().RegisterReflectionAdapter<TA>();
            return this;
        }

        /// <summary>
        /// Paris a given class with each one of its interfaces
        /// </summary>
        /// <typeparam name="TA">The class to be registered</typeparam>
        public Container Register<TA>() {
            foreach (var myType in typeof(TA).GetInterfaces()) {
                var registrationMethod = RegisterPortAdapterMethod.MakeGenericMethod(myType, typeof(TA));
                registrationMethod.Invoke(this, null);
            }
            return this;
        }

        public Container ImportContainer(Container container) {
            foreach (var p in container._ports.Keys)
                _ports.Add(p, container._ports[p]);
            return this;
        }

        /// <summary>
        /// Pairs interface with a static instance of a class (useful for singleton-like needs)
        /// </summary>
        /// <typeparam name="TP">the interface</typeparam>
        /// <param name="instance">the class intance implementing the interface</param>
        public Container Register<TP>(TP instance) {
            GetPortForAGivenContract<TP>().RegisterInstanceAdapter(instance);
            return this;
        }

        /// <summary>
        /// Pairs an interface with a factory method
        /// </summary>
        /// <typeparam name="TP">The interface</typeparam>
        /// <param name="factory">The factory method</param>
        // ReSharper disable once UnusedMethodReturnValue.Global (allows for fluent syntax)
        public Container Register<TP>(Func<Container, TP> factory) {
            GetPortForAGivenContract<TP>().RegisterFactoryAdapter(factory);
            return this;
        }

        /// <summary>
        /// Return True if the given interface has been registered into this container
        /// </summary>
        /// <typeparam name="T">The interface for ask for</typeparam>
        /// <returns></returns>
        public bool IsRegistered<T>() => _ports.ContainsKey(typeof(T));

        #endregion

        #region obtain adapters for given ports

        /// <summary>
        /// Returns an instance of a registered class. Notice the generic version of this method is preferred always.
        /// </summary>
        /// <param name="contract">The interface</param>
        public object GetInstance(Type contract) {
            MethodInfo genericMethod;

            if (!GenericInstanceProviderMethod.TryGetValue(contract, out genericMethod)) {
                genericMethod = GetInstanceMethod.MakeGenericMethod(contract);
                GenericInstanceProviderMethod.Add(contract, genericMethod);
            }

            try {
                return genericMethod.Invoke(this, null);
            }
            catch (TargetInvocationException e) {
                if (e.InnerException != null) throw e.InnerException;
                throw;
            }
        }

        /// <summary>
        /// Returns an instance of a registered class
        /// </summary>
        /// <typeparam name="T">The interface</typeparam>
        public T GetInstance<T>() {
            var myType = typeof(T);
            AdapterProvider<T> adapter;
            try {
                var port = _ports[myType].GetTyped<T>();
                adapter = port.Addapter;
            }
            catch (KeyNotFoundException) {
                throw new UnresolvedInterfaceException(myType);
            }

            return adapter.GrabInstance(this);
        }

        #endregion

        #region Private stuff and implementation detail

        private Port<TP> GetPortForAGivenContract<TP>() {
            Port port;
            if (_ports.TryGetValue(typeof(TP), out port)) return port.GetTyped<TP>();

            var typedPort = new Port<TP>();
            _ports.Add(typeof(TP), typedPort);
            return typedPort;
        }

        private readonly Dictionary<Type, Port> _ports = new Dictionary<Type, Port>();

        #endregion

        #region Reflection catched generic-generated methods from JIT

        private static readonly MethodInfo GetInstanceMethod =
            typeof(Container).GetMethods().First(m =>
                m.Name == "GetInstance" && m.IsGenericMethod && m.GetGenericArguments().Length == 1 &&
                m.GetParameters().Length == 0);

        private static readonly MethodInfo RegisterPortAdapterMethod =
            typeof(Container).GetMethods().First(m =>
                m.Name == "Register" && m.IsGenericMethod && m.GetGenericArguments().Length == 2 &&
                m.GetParameters().Length == 0);

        private static readonly Dictionary<Type, MethodInfo> GenericInstanceProviderMethod =
            new Dictionary<Type, MethodInfo>();

        #endregion
    }
}