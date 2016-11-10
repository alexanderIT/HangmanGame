using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL.DataObject;
using Game.DAL.EFDbContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Game.BL
{
    public class NewUserManager : UserManager<ApplicationUser>
    {
        public NewUserManager()
            : base(new UserStore<ApplicationUser>(new UserDbContext()))
        {

        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return (Store as UserStore<ApplicationUser>).GetUsers();
        }
    }
}
