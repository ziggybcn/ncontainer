using System;
using System.Diagnostics;

namespace NContainer {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    public class UnresolvedInterfaceException : Exception {
        internal UnresolvedInterfaceException(Type dependency) : base(
            $"No class provider was found for the {dependency.Name} interface") {
        }
    }
}