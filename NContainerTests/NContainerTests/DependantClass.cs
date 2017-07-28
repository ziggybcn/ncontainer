using System;

namespace NContainerTests {
    public class DependantClass : DependantInterface {
        public DependantClass(TestInterfaceA myTestClass) {
            if (myTestClass == null)
                throw new Exception("Expecting dependency to be instantiated!");
        }
    }
}