using System;

namespace NContainer {
    public interface Adapter<out T>
    {
        T GrabInstance(Container container);
    }
}