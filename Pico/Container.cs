using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;

namespace Pico {
    public class Container {

        public Container Register<P, A>() where A : P {
            var port = new Port<P>();
            ports.Add(typeof(P), port);
            port.RegisterAddapter<A>();
            return this;
        }

        static MethodInfo registerPortAdapterMethod = typeof(Container).GetMethods().First(m =>
            m.Name == "Register" && m.IsGenericMethod && m.GetGenericArguments().Length == 2);

        public Container Register<A>() 
        {

            foreach (var myType in typeof(A).GetInterfaces())
            {
                var genericMethod = registerPortAdapterMethod.MakeGenericMethod(myType,typeof(A));
                genericMethod.Invoke(this, null); // No target, no arguments
            }
            return this;
        }

        static MethodInfo getInstanceMethod = typeof(Container).GetMethods().First(m =>
            m.Name == "GetInstance" && m.IsGenericMethod && m.GetGenericArguments().Length == 1);

        public object GetInstance(Type contract) {
                var genericMethod = getInstanceMethod.MakeGenericMethod(contract);
                return genericMethod.Invoke(this, null); // No target, no arguments
        }


        public T GetInstance<T>() {
            var myType = typeof(T);
            var port = ports[myType].GetTyped<T>();
            var adapter = port.GetDefaultAddapter();
            return adapter.GrabInstance(this);
        }

        private Dictionary<Type, Port> ports =  new Dictionary<Type, Port>();
        
    }
}