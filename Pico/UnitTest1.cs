using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pico
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    public class Container
    {
        public Container Register(Type myType)
        {
            foreach (var cosa in myType.GetInterfaces())
                Register(myType, cosa);
            return this;
        }

        public Container Register(Type myType, Type myInterface)
        {
            if (!myInterface.IsInterface) throw new ExpectingInterfaceException("Expecting an interface type");
            portsAndAddapters.ContainsKey(myInterface)
            return this;
        }

        //private readonly PortsAndAddapters portsAndAddapters = new PortsAndAddapters();
    }

    internal class Port
    {
        private readonly List<Addapter> addapters = new List<Addapter>();
        public readonly Type Contract;
        public Port(Type contract)
        {
            Contract = contract;
        }
    }

    internal class Addapter
    {
        public Addapter(Type implementation)
        {
            Implementation = implementation;
        }

        public readonly Type Implementation;

    }


    public class ExpectingInterfaceException : Exception
    {
        public ExpectingInterfaceException(string details):base(details) { }

    }


    internal class TestClassA: TestInterfaceA
    {
        TestClassA()
        {
            
        }
    }

    internal class DependantClass : DependantInterface
    {
        DependantClass(TestInterfaceA myTestClass)
        {
            
        }
    }

    public interface TestInterfaceA { }

    public interface DependantInterface { }

}


