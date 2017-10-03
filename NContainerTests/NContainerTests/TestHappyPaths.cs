using System.Collections.Generic;
using FluentAssertions;
using NContainer;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    public class TestHappyPaths {

        [Parallelizable,Test]
        public void RegisterAClassShouldRegisterItsContracts() {
            // Notice that Class Apple implements contract Fruit
            var container = new Container();
            container.Register<Apple>();
            container.GetComponent<Fruit>().Should().BeOfType<Apple>();
        }

        [Parallelizable, Test]
        public void ClassWithDependenciesShouldReturnDependenciesSolved() {
            // Notice that Class Apple implements contract Fruit
            // Class FruitPie implements Desert
            // Deser depends of Fruit to provide Flavor

            var container = new Container();
            container.Register<Apple>().And().Register<FruitPie>();
            container.GetComponent<Desert>().Flavor.Should().BeOfType<Apple>();
        }

        [Parallelizable, Test]
        public void RegisteringInterfaceAndClassShouldReturnTheClass() {
            // Notice that Class Apple implements contract Fruit
            var container = new Container();
            container.Register<Fruit, Apple>();
            container.GetComponent<Fruit>().Should().BeOfType<Apple>();
        }

        [Parallelizable, Test]
        public void InterfaceWithFactoryMethodShouldBeExecuted() {
            // Notice that Class Apple implements contract Fruit
            var container = new Container();
            container.Register<Fruit>(myContainer => new Apple());
            container.GetComponent<Fruit>().Should().BeOfType<Apple>();
        }

        [Parallelizable, Test]
        public void SingleInstanceContractRegistrationShouldReturnSameInstance() {
            // Notice that Class Apple implements contract Fruit
            var container = new Container();
            var myApple = new Apple();
            container.Register<Fruit>(myApple);
            container.GetComponent<Fruit>().Should().BeSameAs(myApple);
        }


        [Parallelizable, Test]
        public void MultipleRequests() {
            // Notice that Class Apple implements contract Fruit
            var container = new Container();
            var myApple = new Apple();
            container.Register<Fruit>(myApple);
            container.Register<Fruit>(myApple);
            var interfaceAVariable = container.GetComponent<Fruit>();
            var anotherInterfaceAVariable = container.GetComponent<Fruit>();
            interfaceAVariable.Should().BeSameAs(anotherInterfaceAVariable);
        }

        [Parallelizable, Test]
        public void IsRegisteredReturnsFalseForUnregisteredInterfaces() {
            new Container().IsRegistered<Fruit>().Should().BeFalse();
        }

        [Parallelizable, Test]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() {
            var sut = new Container();
            sut.Register<Apple>();
            sut.IsRegistered<Fruit>().Should().BeTrue();
        }


        [Parallelizable, Test]
        public void HappyPathWithGenerics()
        {
            var c = new Container();
            c.Register<IEnumerable<string>>(new List<string>());
            c.GetComponent<IEnumerable<string>>().Should().BeOfType<List<string>>();
        }

    }
}