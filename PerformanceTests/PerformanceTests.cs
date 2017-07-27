#region imported references

using NBench;
using NContainer;

#endregion

namespace NContainerTests {
    public class PerformanceTests {
        private const int Iterations = 4000000;

        [PerfBenchmark(NumberOfIterations = 1,
            RunMode = RunMode.Throughput,
            TestMode = TestMode.Test,
            SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 2000)]
        public void MeasureReflectionInstanceResolver() {
            var c = new Container().Register<TestClassA>().Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        public void MeasureSingletoneDependencyInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA()).Register<DependantClass>();
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        public void MeasureSingletoneInstanceResolver() {
            var c = new Container().Register<TestInterfaceA>(new TestClassA());
            c.Register<DependantInterface>(new DependantClass(c.GetInstance<TestInterfaceA>()));
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }

        public void MeasureFactoryInstanceResolver() {
            var c = new Container();
            c.Register<DependantInterface>(container => new DependantClass(container.GetInstance<TestInterfaceA>()));
            c.Register<TestInterfaceA>(container => new TestClassA());
            for (var i = 0; i < Iterations; i++)
                c.GetInstance<DependantInterface>();
        }
    }
}