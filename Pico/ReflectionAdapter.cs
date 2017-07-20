using System.Linq;
using System.Reflection;

namespace Pico {
    internal class ReflectionAdapter<T> : Adapter<T>
    {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {

            if (Constructors.Length == 0)
                throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var parameters = Constructors[0].GetParameters();
            var myParams = parameters.Select(parameter => container.GetInstance(parameter.ParameterType));

            return (T)Constructors[0].Invoke(myParams.ToArray());

        }
    }

    internal class InstanceAdapter<T> : Adapter<T> {
        private readonly T _instance;
        public InstanceAdapter(T instance) {
            _instance = instance;
        }

        public T GrabInstance(Container container) => _instance;
    }

    }
}