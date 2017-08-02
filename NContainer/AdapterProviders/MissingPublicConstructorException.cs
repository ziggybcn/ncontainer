using System;
using System.Diagnostics;

namespace NContainer.AdapterProviders {
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) {
        }
    }
}