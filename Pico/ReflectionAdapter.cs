using System.Linq;
using System.Reflection;

namespace NContainer {
    internal class ReflectionAdapter<T> : Adapter<T>
    {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {

            if (Constructors.Length == 0)
                throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var parameters = Constructors[0].GetParameters(); //We use first constructor
            var myParams = parameters.Select(parameter => container.GetInstance(parameter.ParameterType));

            return (T)Constructors[0].Invoke(myParams.ToArray());

        }
    }
}
