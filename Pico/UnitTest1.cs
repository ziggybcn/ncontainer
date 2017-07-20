﻿using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pico
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1() {
            var c = new Container();
            c.Register<TestInterfaceA, TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var c = new Container();
            c.Register<TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var c = new Container();
            c.Register<TestClassA>();
            c.Register<DependantClass>();
            var myVariable = c.GetInstance<DependantInterface>();
            Assert.IsInstanceOfType(myVariable, typeof(DependantClass));
        }

        [TestMethod]
        public void TestMethod4() {
            var c = new Container();
            c.Register<NonPublicConstructorClass>();
            Assert.ThrowsException<MissingPublicConstructorException>(()=> c.GetInstance<IEnumerable>());

        }
    }

    public class NonPublicConstructorClass : IEnumerable {
        private NonPublicConstructorClass() {
        }

        public IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }
    }

    public class ExpectingInterfaceException : Exception
    {
        public ExpectingInterfaceException(string details):base(details) { }
    }


    internal class TestClassA: TestInterfaceA
    {
        public void SayHello() => Console.WriteLine("Hello world");
    }

    internal class DependantClass : DependantInterface
    {
        public DependantClass(TestInterfaceA myTestClass)
        {
            
        }
    }

    public interface TestInterfaceA {
        void SayHello();
    }

    public interface DependantInterface { }

}


