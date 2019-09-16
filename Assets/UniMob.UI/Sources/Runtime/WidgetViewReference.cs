using System;

namespace UniMob.UI
{
    public struct WidgetViewReference : IEquatable<WidgetViewReference>
    {
        private Atom<WidgetViewReference> _source;
        private WidgetViewReferenceType _type;
        private string _path;

        private WidgetViewReference(WidgetViewReferenceType type, string path)
        {
            _type = type;
            _path = path;
            _source = null;
        }

        internal WidgetViewReference(Atom<WidgetViewReference> source)
        {
            _type = WidgetViewReferenceType.Resource;
            _path = null;
            _source = source;
        }

        internal void LinkAtomToScope()
        {
            _source?.Get();
        }

        public WidgetViewReferenceType Type => _source?.Value.Type ?? _type;
        public string Path => _source?.Value.Path ?? _path;

        public bool Equals(WidgetViewReference other)
        {
            return Type == other.Type && Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            return obj is WidgetViewReference other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked((int) Type * 397) ^ (Path != null ? Path.GetHashCode() : 0);
        }
        
         public override string ToString()
         {
             return $"[{nameof(WidgetViewReference)}: {_type} {_path} {_source}]";
         }

        public static WidgetViewReference Addressable(string path)
        {
            return new WidgetViewReference(WidgetViewReferenceType.Addressable, path);
        }

        public static WidgetViewReference Resource(string path)
        {
            return new WidgetViewReference(WidgetViewReferenceType.Resource, path);
        }
    }

    public enum WidgetViewReferenceType
    {
        Resource,
        Addressable,
    }
}