
namespace NContainer {
    public class UnresolvedInterfaceException : System.Exception {
        internal UnresolvedInterfaceException(System.Type dependency) : base(
            $"No class provider was found for the {dependency.Name} interface") { }
    }
}