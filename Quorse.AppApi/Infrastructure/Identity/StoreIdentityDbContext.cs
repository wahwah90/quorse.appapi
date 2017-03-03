using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Quorse.AppApi.Infrastructure.Identity
{
    public class StoreIdentityDbContext : IdentityDbContext<StoreUser>
    {
        public StoreIdentityDbContext() : base("QuorseIdentityDb")
        {
            Database.SetInitializer<StoreIdentityDbContext>(new StoreIdentityDbInitializer());
        }
        public static StoreIdentityDbContext Create()
        {
            return new StoreIdentityDbContext();
        }
    }
}