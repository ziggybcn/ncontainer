using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NContainer.AdapterProviders {
#if !DEBUG

    [DebuggerStepThrough]
#endif
    internal class ReflectionAdapterProvider<T> : AdapterProvider<T> {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {
            var instance = default(T);
            if (Constructors.Length == 0)
                throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var parameters = Constructors[0].GetParameters(); //By now, we use first constructor
            var myParams = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                myParams[i] = container.GetComponent(parameters[i].ParameterType);
            try {
                instance= (T) Constructors[0].Invoke(myParams);
            }
            catch (TargetInvocationException e) when (e.InnerException != null) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();                
            }
            return instance;
        }
    }
}