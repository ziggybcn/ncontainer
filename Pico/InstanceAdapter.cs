namespace NContainer {
    public class InstanceAdapter<T> : Adapter<T> {
        private readonly T _instance;
        public InstanceAdapter(T instance) {
            _instance = instance;
        }

        public T GrabInstance(Container container) => _instance;
    }
}