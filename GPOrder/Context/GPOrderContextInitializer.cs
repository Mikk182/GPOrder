using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;


// TODO : utiliser ce context pour se loguer
    public class GPOrderContextInitializer : DropCreateDatabaseAlways<GPOrderContext>
    {
        protected override void Seed(GPOrderContext context)
        {
        WebSecurity.Register("Demo", "123456", "demo@demo.com", true, "Demo", "Demo");
        Roles.CreateRole("Admin");
        Roles.AddUserToRole("Demo", "Admin");
        }
    }