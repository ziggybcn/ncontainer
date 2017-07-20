using System;
using System.Collections.Generic;
using System.Linq;

namespace Pico {
    internal class Port
    {
        private readonly List<Addapter> addapters = new List<Addapter>();
        public readonly Type Contract;
        public Port(Type contract)
        {
            Contract = contract;
        }

        public Port RegisterAddapter(Type implementation) {
            addapters.Add(new Addapter(implementation));
            return this;
        }

        public Addapter GetDefaultAddapter() => addapters.First();
    }
}