using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IKeyInstance : IKeyInspector
    {
        void Column(string columnName);
        new void ForeignKey(string constraisnt);
        new void PropertyRef(string property);
        new IEnumerable<IColumnInspector> Columns { get; }
        void CascadeOnDelete();
    }
}