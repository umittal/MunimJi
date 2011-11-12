using munimji.core.persistance;
using munimji.core.persistance.annoations;

namespace munimji.data.model.persons {
    public class Person : EntityBase {
        [MaxSize(100)]
        public virtual string FirstName { get; set; }
        [NullableAttribute]
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
    }

}