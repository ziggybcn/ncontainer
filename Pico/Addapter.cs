using System;

namespace Pico {
    internal class Addapter
    {
        public Addapter(Type implementation)
        {
            Implementation = implementation;
        }

        public readonly Type Implementation;

    }
}