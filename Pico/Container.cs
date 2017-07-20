using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;

namespace NContainer
{
    public class Container
    {

        public Container Register<P, A>() where A : P
        {
            var port = new Port<P>();
            ports.Add(typeof(P), port);
            port.RegisterAdapter<A>();
            return this;
        }

        private static readonly MethodInfo RegisterPortAdapterMethod =
            typeof(Container).GetMethods().First(m =>
                m.Name == "Register" && m.IsGenericMethod && m.GetGenericArguments().Length == 2);

        public Container Register<A>()
        {

            foreach (var myType in typeof(A).GetInterfaces())
            {
                var registrationMethod = RegisterPortAdapterMethod1.MakeGenericMethod(myType, typeof(A));
                registrationMethod.Invoke(this, null);
            }
            return this;
        }

        public Container Register<P>(P instance) {
            var port = new Port<P>();
            ports.Add(typeof(P), port);
            port.RegisterAdapter(instance);
            return this;
        }

        public Container Register<P>(Func<Container,P> factory) {
            var port = new Port<P>();
            ports.Add(typeof(P), port);
            port.RegisterAdapter(factory);
            return this;
        }


        private static readonly MethodInfo GetInstanceMethod =
            typeof(Container).GetMethods().First(m =>
                m.Name == "GetInstance" && m.IsGenericMethod && m.GetGenericArguments().Length == 1);

        public object GetInstance(Type contract)
        {
            var genericMethod = GetInstanceMethod.MakeGenericMethod(contract);
            return genericMethod.Invoke(this, null); 
        }


        public T GetInstance<T>()
        {
            var myType = typeof(T);
            var port = ports[myType].GetTyped<T>();
            var adapter = port.GetDefaultAddapter();
            return adapter.GrabInstance(this);
        }

        private Dictionary<Type, Port> ports = new Dictionary<Type, Port>();

        public static MethodInfo RegisterPortAdapterMethod1 => RegisterPortAdapterMethod;
    }
}