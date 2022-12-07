using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using TheLazyCrazyBrain.CarRental.Module.BusinessObjects;

namespace TheLazyCrazyBrain.CarRental.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();


        ApplicationUser sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
        if (sampleUser == null) {
            sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
            sampleUser.UserName = "User";

            // Set a password if the standard authentication type is used
            sampleUser.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
        }


        PermissionPolicyRole defaultRole = CreateDefaultRole();
        sampleUser.Roles.Add(defaultRole);

        ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
        if (userAdmin == null) {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
        }
        // If a role with the Administrators name doesn't exist in the database, create this role
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if (adminRole == null) {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }
        adminRole.IsAdministrative = true;
        userAdmin.Roles.Add(adminRole);
        ObjectSpace.CommitChanges(); //This line persists created object(s).

        var sampleCountry = ObjectSpace.FirstOrDefault<Country>(c => c.Name == "Österreich");
        if (sampleCountry == null) {
            sampleCountry = ObjectSpace.CreateObject<Country>();
            sampleCountry.Name = "Österreich";
            sampleCountry.PhoneCode = "+43";
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }

        var sampleAddress = ObjectSpace.FirstOrDefault<Address>(a => a.Street == "Hans Wurst Strasse");
        if (sampleAddress == null) {
            sampleAddress = ObjectSpace.CreateObject<Address>();
            sampleAddress.Street = "Hans Wurst Strasse";
            sampleAddress.StateProvince = "Tirol";
            sampleAddress.City = "Innsbruck";
            sampleAddress.Country = sampleCountry;
            sampleAddress.ZipPostal = "6020";
            ObjectSpace.CommitChanges(); //This line persists created object(s).

        }

        var sampleCustomer = ObjectSpace.FirstOrDefault<Customer>(c => c.FirstName == "Hans" && c.LastName == "Wurst");
        if (sampleCustomer == null) {
            sampleCustomer = ObjectSpace.CreateObject<Customer>();

            sampleCustomer.FirstName = "Hans";
            sampleCustomer.LastName = "Wurst";
            sampleCustomer.BirthDate = new DateTime(1995, 02, 21);
            sampleCustomer.Address = sampleAddress;
            sampleCustomer.DefaultTax = 0.2;
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }


        var sampleBrand = ObjectSpace.FirstOrDefault<Brand>(b => b.Name == "Audi");
        if (sampleBrand == null) {
            sampleBrand = ObjectSpace.CreateObject<Brand>();

            sampleBrand.Name = "Audi";

            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }


        var sampleCar = ObjectSpace.FirstOrDefault<Car>(c => c.Name == "A4");
        if (sampleCar == null) {
            sampleCar = ObjectSpace.CreateObject<Car>();

            sampleCar.Name = "A4";
            sampleCar.Brand = sampleBrand;
            sampleCar.NumberPlate = "IL 1337 FU";
            sampleCar.NextServiceDue = DateTime.Now.Date.AddYears(1);

            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }

        var sampleTariff = ObjectSpace.FirstOrDefault<Tariff>(t => t.Car != null && t.Car.Oid == sampleCar.Oid);
        if (sampleTariff == null) {
            sampleTariff = ObjectSpace.CreateObject<Tariff>();
            sampleTariff.Car = sampleCar;
            sampleTariff.DateStart = DateTime.Now.Date.AddYears(-1);
            sampleTariff.PriceNetPerKm = 14.2;
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }

    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateDefaultRole() {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null) {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
