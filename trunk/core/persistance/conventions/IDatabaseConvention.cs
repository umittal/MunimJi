namespace munimji.core.persistance.conventions {
    public interface IDatabaseConvention {
        string Catalog { get; }
        string Schema { get; }
        string TablePrefix { get; }
        string TablePostfix { get; }
    }
}