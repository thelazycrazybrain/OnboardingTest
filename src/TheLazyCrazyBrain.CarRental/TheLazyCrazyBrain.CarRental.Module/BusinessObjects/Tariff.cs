using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent, DefaultClassOptions]
    public class Tariff : BaseObject {
        public Tariff(Session session) : base(session) {

        }

        private DateTime _dateStart;
        [RuleRequiredField("Tariff.DateStartNotEmpty", DefaultContexts.Save, "Date start must not be empty.")]
        public DateTime DateStart { get => _dateStart; set => SetPropertyValue(nameof(DateStart), ref _dateStart, value); }

        // TODO: Should use currency!
        private double _priceNetPerKm;
        public double PriceNetPerKm { get => _priceNetPerKm; set => SetPropertyValue(nameof(PriceNetPerKm), ref _priceNetPerKm, value); }

        private Car _car;
        [RuleRequiredField("Tariff.CarNotNull", DefaultContexts.Save, "Car must not be empty.")]

        [Association("CAR_TARIFFS")]
        public Car Car { get => _car; set => SetPropertyValue(nameof(Car), ref _car, value); }

        public override void AfterConstruction() {
            DateStart = DateTime.Now.Date;
            base.AfterConstruction();
        }
    }


}
