using System;
using System.Collections.Generic;
using System.Linq;

namespace munimji.core.lib.extentions {
    public static class EnumbrableExtender {
        public static bool IsEmpty<T>(this IEnumerable<T> values) {
            try {
                values.First();
                return false;
            }
            catch {
                return true;
            }
        }

        public static bool In<T>(this T lhs, IEnumerable<T> values)
        {
            return values.Contains(lhs);
        }

        public static HashSet<TResult> ToHashSet<T, TResult>(this IEnumerable<T> values, Func<T, TResult> valueSelector)
        {
            var hs = new HashSet<TResult>();
            foreach (var value in values) {
                var key = valueSelector.Invoke(value);
                if (!hs.Contains(key))
                {
                    hs.Add(key);
                }
            }
            return hs;
        }
    }
}