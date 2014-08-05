using System;
using System.Collections.Generic;
using System.Linq;
using builder.Codeplex;

namespace builder
{
    /// <summary>
    /// Optional is a switch with two cases:
    ///     case Value
    ///     case NoValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Optional<T>
    {
        public abstract TR Select<TR>(Func<T, TR> then, Func<TR> else_);

        public sealed class Value: Optional<T>
        {
            public override TR Select<TR>(Func<T, TR> then, Func<TR> else_)
            {
                return then(V);
            }

            public Value(T v)
            {
                V = v;
            }

            readonly T V;
        }

        public sealed class NoValue : Optional<T>
        {
            public override TR Select<TR>(Func<T, TR> then, Func<TR> else_)
            {
                return else_();
            }
        }

        public IEnumerable<T> ToEnum()
        {
            return Select(v => new[] { v }, Enumerable.Empty<T>);
        }

        Optional()
        {
        }
    }

    public static class Optional
    {
        public static Optional<T> OptionalOf<T>(this T value)
        {
            return new Optional<T>.Value(value);
        }
    }
}
