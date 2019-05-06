using System.Runtime.CompilerServices;

namespace UniMob.Async
{
    internal static class Utils
    {
        public static bool SetBool(ref bool field, bool value)
        {
            if (field == value)
                return false;

            field = value;
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}