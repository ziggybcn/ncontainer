using System;
using FluentAssertions;
using NContainer;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Container injection tests")]
    internal class TestsContainerInjectionIntoContainer {
        [Parallelizable, Test]
        public void ContainerInjectionIntoContainer() {
            var parentContainer = new Container();
            var myApple = new Apple();
            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container();
            inheritedContainer.ImportContainer(parentContainer);

            inheritedContainer.GetComponent<Fruit>().Should().BeSameAs(myApple);
        }

        [Parallelizable, Test]
        public void ContainerInjectionWithUpdatingDuplicates() {
            var sut = new Container();
            var auxContainer = new Container();
            sut.Register<Fruit>(new Apple());
            auxContainer.Register<Fruit>(new Pear());

            sut.ImportContainer(auxContainer, ImportOptions.UpdateDuplicates);

            sut.GetComponent<Fruit>().Should().BeOfType<Pear>();
        }

        [Parallelizable, Test]
        public void ContainerInjectionIgnoringDuplicates() {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            var myPear = new Pear();
            auxContainer.Register<Fruit>(myPear).And().Register<FruitPie>();

            sut.ImportContainer(auxContainer, ImportOptions.IgnoreDuplicates)
                .GetComponent<Fruit>()
                .Should()
                .BeOfType<Apple>();
        }

        [Parallelizable, Test]
        public void ContainerInjectionWithUnexpectedDuplicatesThrowsException() {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            auxContainer.Register<Fruit>(new Pear());

            sut.Invoking(
                    container => container.ImportContainer(auxContainer, ImportOptions.ExceptionOnDuplicates))
                .ShouldThrow<ArgumentException>();
        }

        [Parallelizable, Test]
        public void ContainerInjectionIntoContainerConstructor() {
            var parentContainer = new Container();
            var myApple = new Apple();

            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container(parentContainer);

            inheritedContainer.GetComponent<Fruit>().Should().BeSameAs(myApple);
        }
    }
}