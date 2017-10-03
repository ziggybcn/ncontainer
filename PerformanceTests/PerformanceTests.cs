#region imported references

using NBench;
using NContainer;
using NContainerTests.TestScenarioItems;

#endregion

namespace PerformanceTests {
    public class PerformanceTests {
        private const int Iterations = 4000000;

        [PerfBenchmark(NumberOfIterations = 1,
            RunMode = RunMode.Throughput,
            TestMode = TestMode.Test,
            SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 2000)]
        public void MeasureReflectionInstanceResolver() {
            var c = new Container().Register<Apple>().Register<FruitPie>();
            for (var i = 0; i < Iterations; i++)
                c.GetComponent<Desert>();
        }

        public void MeasureSingletoneDependencyInstanceResolver() {
            var c = new Container().Register<Fruit>(new Apple()).Register<FruitPie>();
            for (var i = 0; i < Iterations; i++)
                c.GetComponent<Desert>();
        }

        public void MeasureSingletoneInstanceResolver() {
            var c = new Container().Register<Fruit>(new Apple());
            c.Register<Desert>(new FruitPie(c.GetComponent<Fruit>()));
            for (var i = 0; i < Iterations; i++)
                c.GetComponent<Desert>();
        }

        public void MeasureFactoryInstanceResolver() {
            var c = new Container();
            c.Register<Desert>(container => new FruitPie(container.GetComponent<Fruit>()));
            c.Register<Fruit>(container => new Apple());
            for (var i = 0; i < Iterations; i++)
                c.GetComponent<Desert>();
        }
    }
}