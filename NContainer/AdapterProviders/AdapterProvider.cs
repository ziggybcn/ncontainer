namespace NContainer.AdapterProviders {
    internal interface AdapterProvider<out T> {
        T GrabInstance(Container container);        
    }
}