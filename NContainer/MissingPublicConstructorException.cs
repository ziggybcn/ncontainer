using System;

namespace NContainer {
    public class MissingPublicConstructorException : Exception {
        internal MissingPublicConstructorException(string text) : base(text) { }
    }
}