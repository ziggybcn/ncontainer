using System;

namespace NContainerTests.TestScenarioItems {
    public class FruitPie : Desert {
        private readonly Fruit _flavor;
        public FruitPie(Fruit myFlavor) {
            _flavor = myFlavor ?? throw new Exception("Expecting dependency to be instantiated!");
        }

        public Fruit Flavor => _flavor;
    }
}