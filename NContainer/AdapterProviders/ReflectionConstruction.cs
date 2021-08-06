namespace NContainer.AdapterProviders {
    internal interface ReflectionConstruction<T> {
        ReflectionSolver<T> Reflector {get;
        }
    }
}