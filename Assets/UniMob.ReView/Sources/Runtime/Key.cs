using System;
using JetBrains.Annotations;

namespace UniMob.ReView
{
    public abstract class Key : IEquatable<Key>
    {
        public abstract bool Equals(Key other);
        public sealed override bool Equals(object obj) => Equals(obj as Key);
        public override int GetHashCode() => throw new InvalidOperationException();

        public static Key Of([NotNull] object value) => new ObjectKey(value);

        public static bool operator ==(Key a, Key b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(Key a, Key b) => !a?.Equals(b) ?? !ReferenceEquals(b, null);
    }

    internal sealed class ObjectKey : Key, IEquatable<ObjectKey>
    {
        public object Value { get; }

        internal ObjectKey([NotNull] object value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(Key other) => Equals(other as ObjectKey);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"[Key: {Value}]";

        public bool Equals(ObjectKey other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(other.Value, Value))
                return true;

            return Value.Equals(other.Value);
        }
    }
    
    internal class GlobalKey<T> : Key, IEquatable<GlobalKey<T>>
    {
        public static readonly GlobalKey<T> Instance = new GlobalKey<T>();

        private GlobalKey()
        {
        }

        public override bool Equals(Key other) => Equals(other as GlobalKey<T>);
        public override int GetHashCode() => typeof(T).GetHashCode();
        public override string ToString() => $"[GlobalKey: {typeof(T)}]";

        public bool Equals(GlobalKey<T> other) => ReferenceEquals(other, this);
    }

    public static class GlobalKey
    {
        public static Key Of<T>() where T : Widget => GlobalKey<T>.Instance;
    }
}