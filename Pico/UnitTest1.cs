using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pico
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1() {
            var c = new Container();
            c.Register<TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            myVariable.SayHello();
        }
    }


    public class ExpectingInterfaceException : Exception
    {
        public ExpectingInterfaceException(string details):base(details) { }

    }


    internal class TestClassA: TestInterfaceA
    {
        public TestClassA() {

        }

        public void SayHello() => Console.WriteLine("Hello world");


    }

    internal class DependantClass : DependantInterface
    {
        DependantClass(TestInterfaceA myTestClass)
        {
            
        }
    }

    public interface TestInterfaceA {
        void SayHello();
    }

    public interface DependantInterface { }

}


