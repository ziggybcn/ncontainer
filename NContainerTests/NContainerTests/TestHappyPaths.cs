using System;
using FluentAssertions;
using NContainer;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    public class TestHappyPaths {
        [Test]
        public void ContainerInjectionIntoContainer() {
            var parentContainer = new Container();
            var myApple = new Apple();
            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container();
            inheritedContainer.ImportContainer(parentContainer);

            inheritedContainer.GetAdapter<Fruit>().Should().BeSameAs(myApple);
        }


        [Test]
        public void ContainerInjectionWithUpdatingDuplicates() {
            var sut = new Container();
            var auxContainer = new Container();
            sut.Register<Fruit>(new Apple());
            auxContainer.Register<Fruit>(new Pear());

            sut.ImportContainer(auxContainer, ImportOptions.UpdateDuplicates);

            sut.GetAdapter<Fruit>().Should().BeOfType<Pear>();
        }

        [Test]
        public void ContainerInjectionIgnoringDuplicates() {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            auxContainer.Register<Fruit>(new Pear());
            auxContainer.Register<FruitPie>();

            sut.ImportContainer(auxContainer, ImportOptions.IgnoreDuplicates)
                .GetAdapter<Fruit>()
                .Should()
                .BeOfType<Apple>();
        }

        [Test]
        public void ContainerInjectionWithUnexpectedDuplicatesThrowsException() {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            auxContainer.Register<Fruit>(new Pear());

            sut.Invoking(
                    container => container.ImportContainer(auxContainer, ImportOptions.ExceptionOnDuplicates))
                .ShouldThrow<ArgumentException>();
        }


        [Test]
        public void ContainerInjectionIntoContainerConstructor() {
            var parentContainer = new Container();
            var myApple = new Apple();
            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container(parentContainer);

            inheritedContainer.GetAdapter<Fruit>().Should().BeSameAs(myApple);
        }

        [Test]
        public void HappyPathClassRegistration() {
            var container = new Container();
            container.Register<Apple>();
            container.GetAdapter<Fruit>().Should().BeOfType<Apple>();
        }

        [Test]
        public void HappyPathClassWithDependencies() {
            var container = new Container();
            container.Register<Apple>().Register<FruitPie>();
            container.GetAdapter<Desert>().Flavor.Should().BeOfType<Apple>();
        }

        [Test]
        public void HappyPathInterfaceAndClassPair() {
            var container = new Container();
            container.Register<Fruit, Apple>();
            container.GetAdapter<Fruit>().Should().BeOfType<Apple>();
        }

        [Test]
        public void HappyPathInterfaceWithFactoryMethod() {
            var container = new Container();
            container.Register<Fruit>(myContainer => new Apple());
            container.GetAdapter<Fruit>().Should().BeOfType<Apple>();
        }

        [Test]
        public void HappyPathSingleInstance() {
            var container = new Container();
            var myApple = new Apple();
            container.Register<Fruit>(myApple);
            container.GetAdapter<Fruit>().Should().BeSameAs(myApple);
        }

        [Test]
        public void IsRegisteredReturnsFalseForUnregisteredInterfaces() {
            new Container().IsRegistered<Fruit>().Should().BeFalse();
        }

        [Test]
        public void IsRegisteredReturnsTrueForRegisteredInterfaces() {
            var sut = new Container();
            sut.Register<Apple>();
            sut.IsRegistered<Fruit>().Should().BeTrue();
        }


        [Test]
        public void MultipleRequests() {
            var container = new Container();
            var myApple = new Apple();
            container.Register<Fruit>(myApple);
            container.Register<Fruit>(myApple);
            var interfaceAVariable = container.GetAdapter<Fruit>();
            var anotherInterfaceAVariable = container.GetAdapter<Fruit>();
            interfaceAVariable.Should().BeSameAs(anotherInterfaceAVariable);
        }
    }
}