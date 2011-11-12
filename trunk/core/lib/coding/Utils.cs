using System.Linq;

namespace munimji.core.lib.coding {
    public static class Utils {
        public static int ComputeHashCode(object one, params object[] many) {
            unchecked {
                var hash = ReferenceEquals(one, null) ? 0 : one.GetHashCode();

                return many.Select(obj => ReferenceEquals(obj, null) ? 0 : obj.GetHashCode())
                    .Aggregate(hash, (current, hash2) => (current*397) ^ hash2);
            }
        }
    }
}