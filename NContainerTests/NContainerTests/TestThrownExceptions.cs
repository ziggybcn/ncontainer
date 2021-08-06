using System;
using System.Collections;
using FluentAssertions;
using NContainer;
using NContainer.AdapterProviders;
using NContainerTests.TestScenarioItems;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture, Parallelizable, Category("Exception tests")]
    public class TestThrownExceptions {
        [Test]
        public void ConstructorExceptionIsPropagated() {

            // ExceptionInConstructorClass is a class that throws an exception on its constructor.
            var container = new Container();
            container.Register<ExceptionInConstructorClass>();
            container.Invoking(c => c.GetComponent<Fruit>())
                .ShouldThrow<ExceptionInConstructorClass.TestException>();
        }

        [Test]
        public void MissingDependencyThrowsException() {
            var container = new Container();
            container.Register<FruitPie>();
            Action getInstance = () => container.GetComponent<Desert>();
            getInstance.ShouldThrow<UnresolvedInterfaceException>();
        }

        [Test]
        public void MissingPublicConstructorThrowsException() {
            new Container().Register<NonPublicConstructorClass>()
                .Invoking(container => container.GetComponent<Fruit>())
                .ShouldThrow<MissingPublicConstructorException>();
        }

        [Test]
        public void UnregisteredContractThrowsException() {
            new Container().Invoking(container => container.GetComponent<IEnumerable>())
                .ShouldThrow<UnresolvedInterfaceException>();
        }
         
        [Test]
        public void InvalidImportContainerParameterThrowsException() {
            new Container()
                .Invoking(container => container.ImportContainer(new Container(), (ImportOptions) 1024))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void LazyRegistrationOfASingletonWithExceptionOnConsutructor()
        {
            var c = new Container();
            c.RegisterLazy<Fruit, ExceptionInConstructorClass>();
            c.Invoking(c2 => c2.GetComponent<Fruit>()).ShouldThrow<ExceptionInConstructorClass.TestException>();
        }

        [Test]
        public void MissingPublicConstructorInLazyThrowsException()
        {
            new Container().RegisterLazy<Fruit, NonPublicConstructorClass>()
                .Invoking(container => container.GetComponent<Fruit>())
                .ShouldThrow<MissingPublicConstructorException>();
        }

    }
}