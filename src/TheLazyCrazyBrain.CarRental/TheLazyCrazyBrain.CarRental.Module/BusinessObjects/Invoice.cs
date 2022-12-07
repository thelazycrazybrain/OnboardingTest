using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent, DefaultClassOptions]
    public class Invoice : BaseObject {
        public Invoice(Session session) : base(session) {

        }

        private DateTime _invoiceDate;
        [RuleRequiredField("Invoice.InvoiceDateNotEmpty", DefaultContexts.Save, "Invoice Date must not be empty.")]
        public DateTime InvoiceDate { get => _invoiceDate; set => SetPropertyValue(nameof(InvoiceDate), ref _invoiceDate, value); }

        private DateTime _dueDate;
        public DateTime DueDate { get => _dueDate; set => SetPropertyValue(nameof(DueDate), ref _dueDate, value); }
        private DateTime _paymentDate;
        public DateTime PaymentDate { get => _paymentDate; set => SetPropertyValue(nameof(PaymentDate), ref _paymentDate, value); }
        private RentedCars _rental;
        public RentedCars Rental {
            get => _rental;
            set {
                SetPropertyValue(nameof(Rental), ref _rental, value);
                if (!IsLoading && !IsSaving) {
                    if (_rental != null && PriceNetPerKM == 0) {
                        var tariff = _rental.Car.Tariffs.Where(t => t.DateStart <= DateTime.Today).OrderByDescending(t => t.DateStart).FirstOrDefault();
                        if (tariff != null) {
                            PriceNetPerKM = tariff.PriceNetPerKm;
                        }
                    }
                }
            }
        }

        private int _distanceMeter;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int DistanceMeter {
            get => _distanceMeter;
            set {
                SetPropertyValue(nameof(DistanceMeter), ref _distanceMeter, value);
                OnChanged(nameof(Distance));
            }
        }


        public double AddDistanceKm(double distanceKm) {
            DistanceMeter += (int)(distanceKm * 100d);
            return DistanceMeter * 100d;

        }
        private double _taxPercent;
        public double TaxPercent {
            get => _taxPercent;
            set {
                SetPropertyValue(nameof(TaxPercent), ref _taxPercent, value);
                OnChanged(nameof(PriceSum));
                OnChanged(nameof(TaxSum));
            }
        }


        private double _priceNetPerKM;
        public double PriceNetPerKM {
            get => _priceNetPerKM; set {
                SetPropertyValue(nameof(PriceNetPerKM), ref _priceNetPerKM, value);
                OnChanged(nameof(NetSum));
                OnChanged(nameof(TaxSum));
                OnChanged(nameof(PriceSum));
            }
        }

        [NonPersistent]
        public double NetSum { get => PriceNetPerKM * (DistanceMeter / 100d); }
        [NonPersistent]
        public double TaxSum { get => PriceNetPerKM * TaxPercent; }

        [NonPersistent]
        public double PriceSum { get => NetSum + TaxSum; }

        [NonPersistent, ToolTip("Distance that was driven Kilometers.")]
        [RuleRange("Invoice.DistanceMeterNotZero", DefaultContexts.Save, 1, int.MaxValue, CustomMessageTemplate = "Distance must be greater than 0.")]
        public double Distance { get => DistanceMeter / 100d; set => DistanceMeter = (int)(value * 100d); }


        public override void AfterConstruction() {
            InvoiceDate =DateTime.Now.Date;
            DueDate = InvoiceDate.AddDays(14);
            base.AfterConstruction();
        }
    }


}
