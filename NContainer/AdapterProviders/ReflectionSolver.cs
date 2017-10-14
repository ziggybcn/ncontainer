using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NContainer.AdapterProviders {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    internal class ReflectionSolver<T> {
        private static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        public T GrabInstance(Container container) {
            var instance = default(T);
            if (Constructors.Length == 0)
                throw new MissingPublicConstructorException($"No public constructor found for {typeof(T).Name}");

            var constructorParameters = SolveDependencies(container);

            try {
                instance = (T) Constructors[0].Invoke(constructorParameters);
            }
            catch (TargetInvocationException e) when (e.InnerException != null) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
            return instance;
        }

        private object[] SolveDependencies(Container container)
        {
            var constructorParameters = Constructors[0].GetParameters();
            return InstantiateDependencies(container, constructorParameters);
        }

        private object[] InstantiateDependencies(Container container, 
            IReadOnlyList<ParameterInfo> constructorParameters)
        {
            var dependencies = new object[constructorParameters.Count];
            for (var i = 0; i < constructorParameters.Count; i++)
                dependencies[i] = container.GetComponent(constructorParameters[i].ParameterType);
            return dependencies;
        }
    }
}