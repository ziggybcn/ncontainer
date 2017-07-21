using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NContainer
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HappyPathInterfaceAndClassPair() {
            var c = new Container();
            c.Register<TestInterfaceA, TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void HappyPathClassRegistration()
        {
            var c = new Container();
            c.Register<TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void HappyPathClassWithDependencies() {

            Assert.IsInstanceOfType(
                new Container().Register<TestClassA>().Register<DependantClass>().GetInstance<DependantInterface>(),
                typeof(DependantClass));

        }

        [TestMethod]
        public void MissingDependencyThrowsException() {
            Assert.ThrowsException<UnresolvedInterfaceException>(() => 
            new Container().Register<DependantClass>().GetInstance<DependantInterface>()
            );

        }

        [TestMethod]
        public void HappyPathInterfaceWithFActoryMethod() {
            var c = new Container();
            var myInstance = new TestClassA();
            c.Register<TestInterfaceA>(myContainer=> new TestClassA());
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void HappyPathSingleInstance() {
            var c = new Container();
            var myInstance = new TestClassA();
            c.Register<TestInterfaceA>(myInstance);
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.AreSame(myVariable, myInstance);
        }

        [TestMethod]
        public void MissingPublicConstructorThrowsException() {
            var c = new Container();
            c.Register<NonPublicConstructorClass>();
            Assert.ThrowsException<MissingPublicConstructorException>(()=> c.GetInstance<IEnumerable>());
        }

        [TestMethod]
        public void UnregisteredContractThrowsException() {
            var c = new Container();
            Assert.ThrowsException<UnresolvedInterfaceException>(() => c.GetInstance<IEnumerable>());
        }


        //[TestMethod]
        //public void MeasureInstanceResolver() {
        //    var c = new Container().Register<TestClassA>().Register<DependantClass>();
        //    //var c = new Container().Register<TestInterfaceA>(new TestClassA()).Register<DependantClass>();
        //    //var c = new Container().Register<DependantInterface>(container => new DependantClass(new TestClassA()));
        //    for (var i = 0; i < 10000; i++)
        //        for (var j = 0; j < 1000; j++)
        //            c.GetInstance<DependantInterface>();
        //}



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
            if (myTestClass == null) throw new Exception("Expecting dependency to be instantiated!");
        }
    }

    public interface TestInterfaceA {
        void SayHello();
    }

    public interface DependantInterface { }

}


