using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [Persistent, DefaultClassOptions]
    public class Customer : BaseObject {
        public Customer(Session session) : base(session) {
        }

        private Address _address;
        public Address Address { get => _address; set => SetPropertyValue(nameof(Address), ref _address, value); }

        private bool _isActive;
        public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(IsActive), ref _isActive, value); }

        private string _firstName;
        public string FirstName { get => _firstName; set => SetPropertyValue(nameof(FirstName), ref _firstName, value); }

        private string _lastName;
        public string LastName { get => _lastName; set => SetPropertyValue(nameof(LastName), ref _lastName, value); }

        private DateTime? _birthDate;
        public DateTime? BirthDate { get => _birthDate; set => SetPropertyValue(nameof(BirthDate), ref _birthDate, value); }
        private bool _isCompany;
        public bool IsCompany { get => _isCompany; set => SetPropertyValue(nameof(IsCompany), ref _isCompany, value); }

        private double _defaultTax;
        public double DefaultTax { get => _defaultTax; set => SetPropertyValue(nameof(DefaultTax), ref _defaultTax, value); }

        public XPCollection<RentedCars> RentedCars { get => GetCollection<RentedCars>(nameof(RentedCars)); }

        public override void AfterConstruction() {
            IsActive = true;
            base.AfterConstruction();
        }
    }


    [Persistent, DefaultClassOptions]
    public class RentedCars : BaseObject {
        public RentedCars(Session session) : base(session) {

        }
        private Customer _customer;

        [RuleRequiredField("RentedCars.CustomerNotEmpty", DefaultContexts.Save, "Customer must not be empty.")]
        public Customer Customer { get => _customer; set => SetPropertyValue(nameof(Customer), ref _customer, value); }

        private Car _car;
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
    }

    [Persistent]
    public class Brand : BaseObject {
        public Brand(Session session) : base(session) {

        }

        private string _name;
        [RuleRequiredField("Brand.NameNotEmpty", DefaultContexts.Save, "Name must not be empty.")]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value); }

    }

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
                if (_rental != null && PriceNetPerKM == 0) {
                    var tariff = _rental.Car.Tariffs.Where(t => t.DateStart <= DateTime.Today).OrderByDescending(t => t.DateStart).FirstOrDefault();
                    if (tariff != null) {
                        PriceNetPerKM = tariff.PriceNetPerKm;
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
