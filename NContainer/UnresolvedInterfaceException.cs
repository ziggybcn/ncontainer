using System;

namespace NContainer {
    public class UnresolvedInterfaceException : Exception {
        internal UnresolvedInterfaceException(Type dependency):base($"No class provider was found for the {dependency.Name} interface") { }
    }
}