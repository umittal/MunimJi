using munimji.core.persistance.conventions;

namespace munimji.core.persistance
{
    public static class EntityFrameworkGlobals
    {
        public static IDatabaseConvention DefaultConvention { get; set; }
    }
}
