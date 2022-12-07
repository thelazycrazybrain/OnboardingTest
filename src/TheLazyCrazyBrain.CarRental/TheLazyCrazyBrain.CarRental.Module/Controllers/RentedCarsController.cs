using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLazyCrazyBrain.CarRental.Module.BusinessObjects;

namespace TheLazyCrazyBrain.CarRental.Module.Controllers {
    public class RentedCarsController : ViewController {
        public SimpleAction ReturnCarAction { get; protected set; }

        public RentedCarsController() {
            TargetObjectType = typeof(RentedCars);

            ReturnCarAction = new SimpleAction(this, "RentedCarsController.ReturnCarAction", DevExpress.Persistent.Base.PredefinedCategory.Edit.ToString(), OnReturnCar) {
                Caption = "Return car",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
        }


        private void OnReturnCar(object sender, SimpleActionExecuteEventArgs x) {
            if (View.SelectedObjects.Count > 0) {
                var rc = (RentedCars)View.SelectedObjects[0];
                var invoiceObjectSpace = Application.CreateObjectSpace();

                var cr = new CarReturn(rc.ApproxDistance);
                var carReturnObjectSpace = Application.CreateObjectSpace();
                var listView = Application.CreateDetailView(carReturnObjectSpace, cr);

                Application.ShowViewStrategy.ShowViewInPopupWindow(listView, () => {
                    var invoice = invoiceObjectSpace.CreateObject<Invoice>();
                    invoice.AddDistanceKm(cr.DrivenKilometers);
                    rc.ApproxDistance= cr.DrivenKilometers;
                    rc.RentEnd = cr.DateReturned;
                    ObjectSpace.CommitChanges(); // Commit changes on current view -> Rented car is complete
                    invoice.Rental = invoiceObjectSpace.GetObject( rc); // Move rental to invoice objectspace

                    var dv = Application.CreateDetailView(invoiceObjectSpace, invoice);
                    var svp = new ShowViewParameters(dv);
                    svp.NewWindowTarget = NewWindowTarget.Separate;
                    svp.TargetWindow = TargetWindow.NewModalWindow;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, ReturnCarAction)); // Show the invoice

                }, null , "Return", "Cancel" );

            }
            
        }
    }
}
