using Game.DAL.DataObject;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Game.DAL.EFDbContext
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext() : base("DefaultConnection")
        {
        }
    }
}
