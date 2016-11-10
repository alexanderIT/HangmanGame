using Microsoft.AspNet.Identity.EntityFramework;

namespace Game.DAL.DataObject
{
    public class ApplicationUser : IdentityUser
    {
        public int Score { get; set; }
    }
}
