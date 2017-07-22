using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NContainer;

namespace NContainerTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void HappyPathInterfaceAndClassPair() {
            var c = new Container();
            c.Register<TestInterfaceA, TestClassA>();
            var myVariable = c.GetInstance<TestInterfaceA>();
            Assert.IsInstanceOfType(myVariable, typeof(TestClassA));
        }

        [TestMethod]
        public void HappyPathClassRegistration() {
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
                new Container().Register<DependantClass>().GetInstance<DependantInterface>());

        }

        [TestMethod]
        public void HappyPathInterfaceWithFactoryMethod() {
            var c = new Container();
            c.Register<TestInterfaceA>(myContainer => new TestClassA());
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
        public void MultipleRequests() {
            var c = new Container();
            var myInstance = new TestClassA();
            c.Register<TestInterfaceA>(myInstance);
            c.Register<TestInterfaceA>(myInstance);
            var myVariable = c.GetInstance<TestInterfaceA>();
            var myVariableb = c.GetInstance<TestInterfaceA>();
            Assert.AreSame(myVariable, myVariableb);
        }

        [TestMethod]
        public void IsRegisteredReturnsFalse() {
            var c = new Container();
            Assert.IsFalse(c.IsRegistered<TestInterfaceA>());
        }

        [TestMethod]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() {
            var c = new Container();
            c.Register(new TestClassA());
            Assert.IsFalse(c.IsRegistered<TestInterfaceA>());
        }



        [TestMethod]
        public void MissingPublicConstructorThrowsException() {
            var c = new Container();
            c.Register<NonPublicConstructorClass>();
            Assert.ThrowsException<MissingPublicConstructorException>(() => c.GetInstance<TestInterfaceA>());
        }

        [TestMethod]
        public void UnregisteredContractThrowsException() {
            var c = new Container();
            Assert.ThrowsException<UnresolvedInterfaceException>(() => c.GetInstance<IEnumerable>());
        }


    }

    [TestClass]
    public class PerformanceTests {
        private const int Iterations = 4000000;

        [TestMethod]
        public void MeasureReflectionInstanceResolver() {
            var c = new Container().Register<TestClassA>().Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [TestMethod]
        public void MeasureSingletoneDependencyInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA()).Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [TestMethod]
        public void MeasureSingletoneInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA());
            c.Register<DependantInterface>(new DependantClass(c.GetInstance<TestInterfaceA>()));
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [TestMethod]
        public void MeasureFactoryInstanceResolver() {
            var c = new Container();
            c.Register<DependantInterface>(container => new DependantClass(container.GetInstance<TestInterfaceA>()));
            c.Register<TestInterfaceA>(container => new TestClassA());
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

    }
}

