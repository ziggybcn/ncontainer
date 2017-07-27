using System;
using System.Collections;
using FluentAssertions;
using NContainer;
using NContainer.AdapterProviders;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    public class UnitTests {
        [Test]
        public void ContainerInjectionIntoContainer() {
            var parentContainer = new Container();
            var myInstance = new TestClassA();
            parentContainer.Register<TestInterfaceA>(myInstance);

            var inheritedContainer = new Container();
            inheritedContainer.ImportContainer(parentContainer);

            inheritedContainer.GetInstance<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void ContainerInjectionIntoContainerConstructor() {
            var parentContainer = new Container();
            var myInstance = new TestClassA();
            parentContainer.Register<TestInterfaceA>(myInstance);

            var inheritedContainer = new Container(parentContainer);

            inheritedContainer.GetInstance<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void HappyPathClassRegistration() {
            var container = new Container();
            container.Register<TestClassA>();
            container.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathClassWithDependencies() {
            var container = new Container();
            container.Register<TestClassA>().Register<DependantClass>();
            container.GetInstance<DependantInterface>().Should().BeOfType<DependantClass>();
        }

        [Test]
        public void HappyPathInterfaceAndClassPair() {
            var container = new Container();
            container.Register<TestInterfaceA, TestClassA>();
            container.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathInterfaceWithFactoryMethod() {
            var container = new Container();
            container.Register<TestInterfaceA>(myContainer => new TestClassA());
            container.GetInstance<TestInterfaceA>().Should().BeOfType<TestClassA>();
        }

        [Test]
        public void HappyPathSingleInstance() {
            var container = new Container();
            var myInstance = new TestClassA();
            container.Register<TestInterfaceA>(myInstance);
            container.GetInstance<TestInterfaceA>().Should().BeSameAs(myInstance);
        }

        [Test]
        public void IsRegisteredReturnsFalseForUnregisteredInterfaces() {
            new Container().IsRegistered<TestInterfaceA>().Should().BeFalse();
        }

        [Test]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() {
            new Container().Register<TestInterfaceA>(new TestClassA()).IsRegistered<TestInterfaceA>().Should().BeTrue();
        }

        [Test]
        public void MissingDependencyThrowsException() {
            var container = new Container();
            container.Register<DependantClass>();
            Action getInstance = () => container.GetInstance<DependantInterface>();
            getInstance.ShouldThrow<UnresolvedInterfaceException>();
        }

        [Test]
        public void MissingPublicConstructorThrowsException() {
            new Container().Register<NonPublicConstructorClass>()
                .Invoking(container => container.GetInstance<TestInterfaceA>())
                .ShouldThrow<MissingPublicConstructorException>();
        }

        [Test]
        public void MultipleRequests() {
            var container = new Container();
            var myInstance = new TestClassA();
            container.Register<TestInterfaceA>(myInstance);
            container.Register<TestInterfaceA>(myInstance);
            var interfaceAVariable = container.GetInstance<TestInterfaceA>();
            var anotherInterfaceAVariable = container.GetInstance<TestInterfaceA>();
            interfaceAVariable.Should().BeSameAs(anotherInterfaceAVariable);
        }

        [Test]
        public void UnregisteredContractThrowsException() {
            new Container().Invoking(container => container.GetInstance<IEnumerable>())
                .ShouldThrow<UnresolvedInterfaceException>();
        }
    }
}