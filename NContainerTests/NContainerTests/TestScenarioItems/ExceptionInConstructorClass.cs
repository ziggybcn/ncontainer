using System;

namespace NContainerTests.TestScenarioItems {
    internal class ExceptionInConstructorClass : Fruit {
        public ExceptionInConstructorClass() {
            throw new TestException();
        }

        public class TestException : Exception {
        }
    }
}