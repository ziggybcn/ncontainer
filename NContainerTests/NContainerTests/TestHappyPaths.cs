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
            var container = new Container();
            container.Register<Apple>();
            container.IsRegistered<Fruit>().Should().BeTrue();
        }


        [Parallelizable, Test]
        public void HappyPathWithGenerics()
        {
            var container = new Container();
            container.Register<IEnumerable<string>>(new List<string>());
            container.GetComponent<IEnumerable<string>>().Should().BeOfType<List<string>>();
        }


        [Test]
        public void LazyRegistrationWithFactoryMethod() {
            var container = new Container();
            container.RegisterLazy<Desert>(i => new FruitPie(i.GetComponent<Fruit>()));
            container.Register<Fruit, Apple>();
            container.GetComponent<Desert>().Flavor.Should().BeOfType<Apple>();
        }


        [Test]
        public void LazyRegistrationOfASingletonUsingReflection()
        {
            var container = new Container();
            container.RegisterLazy<Desert, FruitPie>();
            container.Register<Fruit, Apple>();
            container.GetComponent<Desert>().Flavor.Should().BeOfType<Apple>();
            container.GetComponent<Desert>().Should().BeSameAs(container.GetComponent<Desert>());
        }

        [Test]
        public void RegisterCloneAndRegister() {
            var myContainer = new Container();
            myContainer.RegisterLazy<Desert, FruitPie>();
            myContainer.Register<Fruit, Apple>();

            myContainer.Clone().And().Register<Fruit, Pear>().And().
                GetComponent<Desert>().Flavor.Should().BeOfType<Pear>();
        }

    }
}