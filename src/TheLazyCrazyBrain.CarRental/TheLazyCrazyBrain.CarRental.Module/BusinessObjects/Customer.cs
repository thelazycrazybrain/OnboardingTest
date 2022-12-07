using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Microsoft.CodeAnalysis.Operations;
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
        [Association("CUSTOMER_RENTEDCARS")]
        public XPCollection<RentedCars> RentedCars { get => GetCollection<RentedCars>(nameof(RentedCars)); }

        public override void AfterConstruction() {
            IsActive = true;
            base.AfterConstruction();
        }
    }


}
