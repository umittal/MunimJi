using munimji.core.persistance;

namespace munimji.data.model.staticData {
    public class BankAccount : EntityBase {
        public virtual string AccountNumber { get; set; }
    }
}