
namespace NContainer.AdapterProviders {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class ReflectionAdapterProvider<T> : AdapterProvider<T>, ReflectionConstruction<T> {

        public T GrabInstance(Container container) => Reflector.GrabInstance(container);

        public ReflectionSolver<T> Reflector { get; } = new ReflectionSolver<T>();
    }
}