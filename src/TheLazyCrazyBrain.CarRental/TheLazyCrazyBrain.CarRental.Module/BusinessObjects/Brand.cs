using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent]
    public class Brand : BaseObject {
        public Brand(Session session) : base(session) {

        }

        private string _name;
        [RuleRequiredField("Brand.NameNotEmpty", DefaultContexts.Save, "Name must not be empty.")]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value); }

    }


}
