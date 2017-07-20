using System;

namespace Pico {
    public interface Adapter<out T>
    {
        T GrabInstance(Container container);
    }
}