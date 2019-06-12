/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
namespace Tbasic.Types
{
    internal static partial class TypeUtil
    {
        public static string NameOf(object o)
        {
            return o?.GetType().Name ?? "null";
        }

        public static object ConvertFromString(string str)
        {
            // now we've just got to parse the supported types until we find a match...
            if (Number.TryParse(str, out Number n))
                return n;
            if (bool.TryParse(str, out bool b))
                return b;
            return null;
        }
    }
}