using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NContainer;
using NContainer.AdapterProviders;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture]
    [Parallelizable]
    [Category("Basic exception tests")]
    public class TestThrownExceptions {
        [Test]
        public void ConstructorExceptionIsPropagated() {
            var container = new Container();
            container.Register<ExceptionInConstructorClass>();
            container.Invoking(c => c.GetAdapter<Fruit>())
                .ShouldThrow<ExceptionInConstructorClass.TestException>();
        }

        [Test]
        public void HappyPathWithGenerics() {
            var c = new Container();
            c.Register<IEnumerable<string>>(new List<string>());
            c.GetAdapter<IEnumerable<string>>().Should().BeOfType<List<string>>();
        }

        [Test]
        public void MissingDependencyThrowsException() {
            var container = new Container();
            container.Register<FruitPie>();
            Action getInstance = () => container.GetAdapter<Desert>();
            getInstance.ShouldThrow<UnresolvedInterfaceException>();
        }

        [Test]
        public void MissingPublicConstructorThrowsException() {
            new Container().Register<NonPublicConstructorClass>()
                .Invoking(container => container.GetAdapter<Fruit>())
                .ShouldThrow<MissingPublicConstructorException>();
        }

        [Test]
        public void UnregisteredContractThrowsException() {
            new Container().Invoking(container => container.GetAdapter<IEnumerable>())
                .ShouldThrow<UnresolvedInterfaceException>();
        }

        [Test]
        public void InvalidImportContainerParameterThrowsException() {
            new Container()
                .Invoking(container => container.ImportContainer(new Container(), (ImportOptions) 1024))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}