namespace NContainer {
    internal interface AdapterProvider<out T> {
        T GrabInstance(Container container);
    }
}