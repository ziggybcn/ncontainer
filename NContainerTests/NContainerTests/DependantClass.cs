using System;

namespace NContainerTests {
    internal class DependantClass : DependantInterface
    {
        public DependantClass(TestInterfaceA myTestClass)
        {
            if (myTestClass == null) throw new Exception("Expecting dependency to be instantiated!");
        }
    }
}