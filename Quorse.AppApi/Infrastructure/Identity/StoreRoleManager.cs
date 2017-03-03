using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Quorse.AppApi.Infrastructure.Identity
{
    public class StoreRoleManager : RoleManager<StoreRole>
    {
        public StoreRoleManager(RoleStore<StoreRole> store) : base(store) { }

        public static StoreRoleManager Create(IdentityFactoryOptions<StoreRoleManager> options, IOwinContext context)
        {
            return new StoreRoleManager(new RoleStore<StoreRole>(context.Get<StoreIdentityDbContext>()));
        }
    }
   
}
