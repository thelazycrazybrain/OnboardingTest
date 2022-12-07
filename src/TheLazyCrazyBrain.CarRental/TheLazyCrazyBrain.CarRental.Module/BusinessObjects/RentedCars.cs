using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent, DefaultClassOptions]
    public class RentedCars : BaseObject {
        public RentedCars(Session session) : base(session) {

        }
        private Customer _customer;
        [Association("CUSTOMER_RENTEDCARS")]
        [RuleRequiredField("RentedCars.CustomerNotEmpty", DefaultContexts.Save, "Customer must not be empty.")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private Car _car;
        [Association("CAR_RENTEDCARS")]
        [RuleRequiredField("RentedCars.CarNotEmpty", DefaultContexts.Save, "Car must not be empty.")]
        public Car Car { get => _car; set => SetPropertyValue(nameof(Car), ref _car, value); }

        private DateTime _rentStart;
        [RuleRequiredField("RentedCars.RentStartNotEmpty", DefaultContexts.Save, "Rent start must not be empty.")]
        public DateTime RentStart { get => _rentStart; set => SetPropertyValue(nameof(RentStart), ref _rentStart, value); }

        private DateTime? _probableRentEnd;
        public DateTime? ProbableRentEnd { get => _probableRentEnd; set => SetPropertyValue(nameof(ProbableRentEnd), ref _probableRentEnd, value); }

        private DateTime? _rentEnd;
        public DateTime? RentEnd { get => _rentEnd; set => SetPropertyValue(nameof(RentEnd), ref _rentEnd, value); }

        private int _approxDistanceMeter;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]

        public int ApproxDistanceMeter {
            get => _approxDistanceMeter;
            set {
                SetPropertyValue(nameof(ApproxDistanceMeter), ref _approxDistanceMeter, value);
                OnChanged(nameof(ApproxDistance));
            }
        }

        [NonPersistent, ToolTip("Distance that will be driven Kilometers.")]
        [RuleRange("RentedCars.RentApproxDistanceMeterNotZero", DefaultContexts.Save, 1, int.MaxValue, CustomMessageTemplate = "Approximate Distance must be greater than 0.")]
        public double ApproxDistance { get => ApproxDistanceMeter / 100d; set => ApproxDistanceMeter = (int)(value * 100d); }

    }


}
