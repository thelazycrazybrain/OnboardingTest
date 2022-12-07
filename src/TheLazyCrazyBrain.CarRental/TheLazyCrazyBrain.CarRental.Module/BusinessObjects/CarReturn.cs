using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Office.Services.Implementation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLazyCrazyBrain.CarRental.Module.BusinessObjects {
    [NonPersistent]
    [DomainComponent]
    public class CarReturn : NonPersistentBaseObject {
        public CarReturn(double drivenKilometers) : base() {
            DateReturned = DateTime.Today;
            DrivenKilometers = drivenKilometers;
        }

        private DateTime _dateReturned;
        public DateTime DateReturned { get => _dateReturned; set => SetPropertyValue(ref _dateReturned, value, nameof(DateReturned)); }

        private double _drivenKilometers;
        public double DrivenKilometers { get => _drivenKilometers; set => SetPropertyValue(ref _drivenKilometers, value, nameof(DrivenKilometers)); }

    }
}
