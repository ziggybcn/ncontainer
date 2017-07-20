using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Pico {
    public class Container
    {
        public Container Register(Type adapter)
        {
            foreach (var cosa in adapter.GetInterfaces())
                Register(adapter, cosa);
            return this;
        }

        public Container Register<T>() {
            return Register(typeof(T));
        }

        public Container Register(Type adapter, Type port)
        {
            if (!port.IsInterface) throw new ExpectingInterfaceException("Expecting an interface type");
            Port internalPort;
            if (ports.ContainsKey(port)) {
                internalPort = ports[port];
            } else {
                internalPort = new Port(port);
                ports.Add(port,internalPort);  
            }
            internalPort.RegisterAddapter(adapter);
            return this;
        }


        public T GetInstance<T>() {
            var myType = typeof(T);
            var port = ports[myType];
            var adapter = port.GetDefaultAddapter();
            var implementation = adapter.Implementation;
            var item = Activator.CreateInstance(implementation);
            return (T)item;
        }

        private Dictionary<Type, Port> ports = new Dictionary<Type,Port>();



    }
}