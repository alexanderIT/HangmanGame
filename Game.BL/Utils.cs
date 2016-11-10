using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL.DataObject;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Game.BL
{
    public static class Utils
    {
        public static IDbSet<ApplicationUser> GetUsers(this UserStore<ApplicationUser> userStore)
        {
            return (userStore.Context as IdentityDbContext<ApplicationUser>).GetUsers();
        }

        public static IDbSet<ApplicationUser> GetUsers(this IdentityDbContext<ApplicationUser> context)
        {
            return context.Users;
        }
    }
}
