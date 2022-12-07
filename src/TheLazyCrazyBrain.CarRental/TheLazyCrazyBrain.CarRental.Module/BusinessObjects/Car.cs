using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent, DefaultClassOptions]
    public class Car : BaseObject {
        public Car(Session session) : base(session) {

        }

        private string _name;
        [RuleRequiredField("Car.NameNotEmpty", DefaultContexts.Save, "Name must not be empty.")]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value); }

        private Brand _brand;
        [RuleRequiredField("Car.BrandNotNull", DefaultContexts.Save, "Brand must not be empty.")]
        public Brand Brand { get => _brand; set => SetPropertyValue(nameof(Brand), ref _brand, value); }

        private string _numberPlate;
        [RuleRequiredField("Car.NumberPlateNotEmpty", DefaultContexts.Save, "Number plate must not be empty.")]
        public string NumberPlate { get => _numberPlate; set => SetPropertyValue(nameof(NumberPlate), ref _numberPlate, value); }

        private bool _isActive;
        public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(IsActive), ref _isActive, value); }


        private int _distanceMeter;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int DistanceMeter {
            get => _distanceMeter;
            set {
                SetPropertyValue(nameof(DistanceMeter), ref _distanceMeter, value);
                OnChanged(nameof(Distance));
            }
        }

        [NonPersistent, ToolTip("Driven Distance in Kilometers.")]
        public double Distance { get => DistanceMeter / 100d; set => DistanceMeter = (int)(value * 100d); }


        private DateTime? _nextServiceDue;
        [ToolTip("If not empty, the application will notify you about oncoming service times.")]
        public DateTime? NextServiceDue { get => _nextServiceDue; set => SetPropertyValue(nameof(NextServiceDue), ref _nextServiceDue, value); }

        [Association("CAR_TARIFFS")]
        public XPCollection<Tariff> Tariffs { get => GetCollection<Tariff>(nameof(Tariffs)); }

        public override void AfterConstruction() {
            IsActive = true;
            base.AfterConstruction();
        }

        [Association("CAR_RENTEDCARS")]
        public XPCollection<RentedCars> RentedCars { get => GetCollection<RentedCars>(nameof(RentedCars)); }
    }


}
