using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NContainer;
using NContainer.AdapterProviders;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    public class TestHappyPaths {
        [Test]
        public void ContainerInjectionIntoContainer() {
            var parentContainer = new Container();
            var myInstance = new TestClassA();
            parentContainer.Register<TestInterfaceA>(myInstance);

            var inheritedContainer = new Container();
            inheritedContainer.ImportContainer(parentContainer);

            inheritedContainer.GetAdapter<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void ContainerInjectionIntoContainerConstructor() {
            var parentContainer = new Container();
            var myInstance = new TestClassA();
            parentContainer.Register<TestInterfaceA>(myInstance);

            var inheritedContainer = new Container(parentContainer);

            inheritedContainer.GetAdapter<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void HappyPathClassRegistration() {
            var container = new Container();
            container.Register<TestClassA>();
            container.GetAdapter<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathClassWithDependencies() {
            var container = new Container();
            container.Register<TestClassA>().Register<DependantClass>();
            container.GetAdapter<DependantInterface>().Should().BeOfType<DependantClass>();
        }

        [Test]
        public void HappyPathInterfaceAndClassPair() {
            var container = new Container();
            container.Register<TestInterfaceA, TestClassA>();
            container.GetAdapter<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathInterfaceWithFactoryMethod() {
            var container = new Container();
            container.Register<TestInterfaceA>(myContainer => new TestClassA());
            container.GetAdapter<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathSingleInstance() {
            var container = new Container();
            var myInstance = new TestClassA();
            container.Register<TestInterfaceA>(myInstance);
            container.GetAdapter<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void IsRegisteredReturnsFalseForUnregisteredInterfaces() {
            new Container().IsRegistered<TestInterfaceA>().Should().BeFalse();
        }

        [Test]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() {
            new Container().Register<TestClassA>().IsRegistered<TestInterfaceA>().Should().BeTrue();
        }


        [Test]
        public void MultipleRequests() {
            var container = new Container();
            var myInstance = new TestClassA();
            container.Register<TestInterfaceA>(myInstance);
            container.Register<TestInterfaceA>(myInstance);
            var interfaceAVariable = container.GetAdapter<TestInterfaceA>();
            var anotherInterfaceAVariable = container.GetAdapter<TestInterfaceA>();
            interfaceAVariable.Should().BeSameAs(anotherInterfaceAVariable);
        }
    }

    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    public class TestThrownExceptions {
        [Test]
        public void ConstructorExceptionIsPropagated() {
            var container = new Container();
            container.Register<ExceptionInConstructorClass>();
            container.Invoking(c => c.GetAdapter<TestInterfaceA>())
                .ShouldThrow<ExceptionInConstructorClass.TestException>();
        }

        [Test]
        public void MissingDependencyThrowsException() {
            var container = new Container();
            container.Register<DependantClass>();
            Action getInstance = () => container.GetAdapter<DependantInterface>();
            getInstance.ShouldThrow<UnresolvedInterfaceException>();
        }

        [Test]
        public void MissingPublicConstructorThrowsException() {
            new Container().Register<NonPublicConstructorClass>()
                .Invoking(container => container.GetAdapter<TestInterfaceA>())
                .ShouldThrow<MissingPublicConstructorException>();
        }

        [Test]
        public void HappyPathWithGenerics() {
            var c = new Container();
            c.Register<IEnumerable<string>>(new List<string>());
            c.GetAdapter<IEnumerable<string>>().Should().BeOfType<List<string>>();
        }

        [Test]
        public void UnregisteredContractThrowsException() {
            new Container().Invoking(container => container.GetAdapter<IEnumerable>())
                .ShouldThrow<UnresolvedInterfaceException>();
        }
    }

    internal class ExceptionInConstructorClass : TestInterfaceA {
        public ExceptionInConstructorClass() {
            throw new TestException();
        }

        public class TestException : Exception {
        }
    }
}