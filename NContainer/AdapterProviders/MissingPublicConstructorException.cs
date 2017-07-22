using System;

namespace NContainer.AdapterProviders {
    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) { }
    }
}