using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using NContainer.AdapterProviders;
using NContainer.Ports;

namespace NContainer {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    public class Container {
        public Container() { }

        /// <summary>
        /// Use this constructor to import data from other container upon container creation.
        /// </summary>
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

        public Container ImportContainer(Container container) => 
            ImportContainer(container, ImportOptions.ExceptionOnDuplicates);

        public Container ImportContainer(Container container, ImportOptions options) {
            switch (options) {
                case ImportOptions.ExceptionOnDuplicates:
                    foreach (var item in container._ports)
                        _ports.Add(item.Key, item.Value);
                    break;
                case ImportOptions.UpdateDuplicates:
                    foreach (var item in container._ports)
                        _ports[item.Key] = item.Value;
                    break;
                case ImportOptions.IgnoreDuplicates:
                    foreach (var item in container._ports)
                        if (_ports.ContainsKey(item.Key) == false)
                            _ports[item.Key] = item.Value;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(options), options,
                        "invalid import option for container.");
            }
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
        /// Creates a singleton instance in a lazy maner. 
        /// The factory method will only be called the first time an instance is requested, 
        /// and a catched copy will be kept for further calls, so it behaves like a factory-based singleton
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <param name="lazyFactory"></param>
        /// <returns></returns>
        public Container RegisterLazy<TP>(Func<Container, TP> lazyFactory) {
            GetPortForAGivenContract<TP>().RegisterLazyFactoryAdapter(this, lazyFactory);
            return this;
        }

        /// <summary>
        /// Paris an interface with a class in lazy-singleton maner. 
        /// That is, the actual instance will be created the first time it is requested, 
        /// and further requests will return a catched copy of it.
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <typeparam name="TA"></typeparam>
        /// <returns></returns>
        public Container RegisterLazy<TP, TA>() where TA : TP {
            GetPortForAGivenContract<TP>().RegisterDeferredSingleton<TA>(this);
            return this;
        }

        /// <summary>
        /// Return True if the given interface has been registered into this container
        /// </summary>
        /// <typeparam name="T">The interface for ask for</typeparam>
        public bool IsRegistered<T>() {
            return _ports.ContainsKey(typeof(T));
        }
        #endregion

        #region obtain adapters for given ports

        /// <summary>
        /// Returns an instance of a registered class. Notice the generic version of this method is preferred always.
        /// </summary>
        /// <param name="contract">The interface</param>
        public object GetComponent(Type contract) {
            var instance = default(object);
            if (!GenericInstanceProviderMethod.TryGetValue(contract, out var genericMethod))
            {
                genericMethod = GetInstanceMethod.MakeGenericMethod(contract);
                GenericInstanceProviderMethod.Add(contract, genericMethod);
            }
            try {
                instance = genericMethod.Invoke(this, null);
            }
            catch (TargetInvocationException e) when (e.InnerException != null) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
            return instance;
        }

        /// <summary>
        /// Returns an instance of a registered class
        /// </summary>
        /// <typeparam name="T">The interface</typeparam>
        public T GetComponent<T>() {
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
            if (_ports.TryGetValue(typeof(TP), out var port)) return port.GetTyped<TP>();

            var typedPort = new Port<TP>();
            _ports.Add(typeof(TP), typedPort);
            return typedPort;
        }

        private readonly Dictionary<Type, Port> _ports = new Dictionary<Type, Port>();

        #endregion

        #region Reflection catched generic-generated methods from JIT

        // ReSharper disable ComplexConditionExpression

        private static readonly MethodInfo GetInstanceMethod =
            typeof(Container).GetMethods()
                .First(m =>
                    m.Name == "GetComponent" && m.IsGenericMethod && m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 0);

        private static readonly MethodInfo RegisterPortAdapterMethod =
            typeof(Container).GetMethods()
                .First(m =>
                    m.Name == "Register" && m.IsGenericMethod && m.GetGenericArguments().Length == 2 &&
                    m.GetParameters().Length == 0);

        // ReSharper restore ComplexConditionExpression

        private static readonly Dictionary<Type, MethodInfo> GenericInstanceProviderMethod =
            new Dictionary<Type, MethodInfo>();

        #endregion
    }
     
    public static class FluentContainerExtensions {
        // ReSharper disable once MethodNameNotMeaningful
        /// <summary>
        /// Returns the container in order to provide flient syntax
        /// </summary>
        public static Container And(this Container container) => container;

        /// <summary>
        /// Creates a soft clone of the current container so it can be used to transitory register adapters.
        /// </summary>
        public static Container Clone(this Container container) => new Container(container);
    }
}