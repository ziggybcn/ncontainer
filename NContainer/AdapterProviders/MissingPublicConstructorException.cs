using System;
using System.Diagnostics;

namespace NContainer.AdapterProviders {
#if IGNORECONTAINER
    [DebuggerStepThrough]
#endif
    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) { }
    }
}