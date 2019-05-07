using System.Collections.Generic;

namespace UniMob.ReView
{
    internal static class ClassPools
    {
        private static readonly Stack<Dictionary<Key, State>> KeyStateDictPool = new Stack<Dictionary<Key, State>>();

        public static Dictionary<Key, State> GetKeyStateDictionary()
            => KeyStateDictPool.Count > 0 ? KeyStateDictPool.Pop() : new Dictionary<Key, State>();

        public static void Recycle(Dictionary<Key, State> dict)
        {
            dict.Clear();
            KeyStateDictPool.Push(dict);
        }
    }
}