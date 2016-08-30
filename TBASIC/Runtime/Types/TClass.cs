// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Parsing;

namespace Tbasic.Runtime
{
    internal class TClass : ObjectContext, ICloneable
    {
        public string Name { get; private set; }
        public TClass ParentClass { get; private set; } = null;
        public LineCollection Constructor { get; private set; }

        public TClass()
        {
        }

        /// <summary>
        /// Generate a base class
        /// </summary>
        public TClass(string name, ObjectContext global) : base(global)
        {
            Name = name;
            Constructor = new LineCollection();
        }

        /// <summary>
        /// Generate a child class
        /// </summary>
        public TClass(string name, TClass parent) : base(parent.ParentContext)
        {
            Name = name;
            ParentClass = parent;
            Constructor = new LineCollection();
        }

        public bool Inherits(TClass parent)
        {
            TClass myancestor = ParentClass;
            while (myancestor != null) {
                if (myancestor == parent)
                    return true;
                myancestor = myancestor.ParentClass;
            }
            return false;
        }

        private Stack<TClass> GetLineage()
        {
            Stack<TClass> lineage = new Stack<TClass>();
            TClass curr = this;
            do {
                lineage.Push(curr);
                curr = curr.ParentClass;
            }
            while (curr != null);
            return lineage;
        }

        public TClass GetInstance(StackData runtime)
        {
            TClass instance = (TClass)Clone();
            instance.SetVariable("@this", instance);
            instance.SetVariable("@base", instance.ParentClass);

            Stack<TClass> lineage = GetLineage();
            do {
                lineage.Pop().Ctor(runtime, instance); // construct all ancestors
            }
            while (lineage.Count > 0);
            return instance;
        }

        private void Ctor(StackData runtime, TClass instance)
        {
            ObjectContext old = runtime.Runtime.Context;
            runtime.Runtime.Context = instance;
            runtime.Runtime.Execute(Constructor); // this is to initialize all the variables

            TBasicFunction ctor;
            if (TryGetFunction("<>ctor", out ctor)) {
                ctor(runtime); // do the user defined constructor
            }

            runtime.Runtime.Context = old;
        }

        internal TClass Clone()
        {
            TClass cloned = CopyFrom<TClass>(this);
            cloned.Name = Name;
            cloned.ParentClass = ParentClass?.Clone();
            cloned.Constructor = Constructor;
            return cloned;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
