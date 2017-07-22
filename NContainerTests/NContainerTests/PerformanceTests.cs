using NContainer;
using NUnit.Framework;

namespace NContainerTests {
    [TestFixture, Parallelizable]
    public class PerformanceTests {
        private const int Iterations = 4000000;

        [Test]
        public void MeasureReflectionInstanceResolver() {
            var c = new Container().Register<TestClassA>().Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [Test]
        public void MeasureSingletoneDependencyInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA()).Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [Test]
        public void MeasureSingletoneInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA());
            c.Register<DependantInterface>(new DependantClass(c.GetInstance<TestInterfaceA>()));
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        [Test]
        public void MeasureFactoryInstanceResolver() {
            var c = new Container();
            c.Register<DependantInterface>(container => new DependantClass(container.GetInstance<TestInterfaceA>()));
            c.Register<TestInterfaceA>(container => new TestClassA());
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

    }
}