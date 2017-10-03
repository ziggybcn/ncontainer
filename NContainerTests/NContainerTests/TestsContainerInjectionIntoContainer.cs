using System;
using FluentAssertions;
using NContainer;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Unit tests")]
    internal class TestsContainerInjectionIntoContainer {
        [Test]
        public void ContainerInjectionIntoContainer()
        {
            var parentContainer = new Container();
            var myApple = new Apple();
            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container();
            inheritedContainer.ImportContainer(parentContainer);

            inheritedContainer.GetComponent<Fruit>().Should().BeSameAs(myApple);
        }


        [Test]
        public void ContainerInjectionWithUpdatingDuplicates()
        {
            var sut = new Container();
            var auxContainer = new Container();
            sut.Register<Fruit>(new Apple());
            auxContainer.Register<Fruit>(new Pear());

            sut.ImportContainer(auxContainer, ImportOptions.UpdateDuplicates);

            sut.GetComponent<Fruit>().Should().BeOfType<Pear>();
        }

        [Test]
        public void ContainerInjectionIgnoringDuplicates()
        {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            auxContainer.Register<Fruit>(new Pear());
            auxContainer.Register<FruitPie>();

            sut.ImportContainer(auxContainer, ImportOptions.IgnoreDuplicates)
                .GetComponent<Fruit>()
                .Should()
                .BeOfType<Apple>();
        }

        [Test]
        public void ContainerInjectionWithUnexpectedDuplicatesThrowsException()
        {
            var sut = new Container();
            sut.Register<Fruit>(new Apple());

            var auxContainer = new Container();
            auxContainer.Register<Fruit>(new Pear());

            sut.Invoking(
                    container => container.ImportContainer(auxContainer, ImportOptions.ExceptionOnDuplicates))
                .ShouldThrow<ArgumentException>();
        }


        [Test]
        public void ContainerInjectionIntoContainerConstructor()
        {
            var parentContainer = new Container();
            var myApple = new Apple();
            parentContainer.Register<Fruit>(myApple);

            var inheritedContainer = new Container(parentContainer);

            inheritedContainer.GetComponent<Fruit>().Should().BeSameAs(myApple);
        }
    }
}