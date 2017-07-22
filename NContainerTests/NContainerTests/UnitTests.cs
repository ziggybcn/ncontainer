using System;
using System.Collections;
using FluentAssertions;
using NContainer;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    public class UnitTests {
        [Test]
        public void HappyPathInterfaceAndClassPair() {
            var c = new Container();
            c.Register<TestInterfaceA, TestClassA>();
            c.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathClassRegistration() {
            var c = new Container();
            c.Register<TestClassA>();
            c.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathClassWithDependencies() {
            var c = new Container();
            c.Register<TestClassA>().Register<DependantClass>();
            c.GetInstance<DependantInterface>().Should().BeOfType<DependantClass>();
        }

        [Test]
        public void MissingDependencyThrowsException() {
            var c = new Container();
            c.Register<DependantClass>();
            Action getInstance = () => c.GetInstance<DependantInterface>();
            getInstance.ShouldThrow<UnresolvedInterfaceException>();

        }

        [Test]
        public void HappyPathInterfaceWithFactoryMethod() {
            var c = new Container();
            c.Register<TestInterfaceA>(myContainer => new TestClassA());
            c.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathSingleInstance() {
            var c = new Container();
            var myInstance = new TestClassA();
            c.Register<TestInterfaceA>(myInstance);
            c.GetInstance<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void MultipleRequests() {
            var c = new Container();
            var myInstance = new TestClassA();
            c.Register<TestInterfaceA>(myInstance);
            c.Register<TestInterfaceA>(myInstance);
            var myVariable = c.GetInstance<TestInterfaceA>();
            var myVariableb = c.GetInstance<TestInterfaceA>();
            Assert.AreSame(myVariable, myVariableb);
        }

        [Test]
        public void IsRegisteredReturnsFalseForUnregisteredInterfaces() => 
            new Container().IsRegistered<TestInterfaceA>().Should().BeFalse();

        

        [Test]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() =>
            new Container().Register<TestInterfaceA>(new TestClassA()).IsRegistered<TestInterfaceA>().Should().BeTrue();
       
        [Test]
        public void MissingPublicConstructorThrowsException() {
            var c = new Container();
            c.Register<NonPublicConstructorClass>();
            Assert.Throws<MissingPublicConstructorException>(() => c.GetInstance<TestInterfaceA>());
        }

        [Test]
        public void UnregisteredContractThrowsException() {
            var c = new Container();
            Assert.Throws<UnresolvedInterfaceException>(() => c.GetInstance<IEnumerable>());
        }
    }
}

