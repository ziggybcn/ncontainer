using System.Reflection;

namespace NContainer.AdapterProviders {
    internal class ReflectionAdapterProvider<T> : AdapterProvider<T> {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {
            if (Constructors.Length == 0)
                throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var parameters = Constructors[0].GetParameters(); //We use first constructor
            var myParams = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                myParams[i] = container.GetAdapter(parameters[i].ParameterType);
            return (T) Constructors[0].Invoke(myParams);
        }
    }
}