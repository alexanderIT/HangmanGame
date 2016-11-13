using Microsoft.AspNet.Identity.EntityFramework;

namespace Game.DAL.DataObject
{
    public class ApplicationUser : IdentityUser
    {
        public int Score { get; set; }

        public int WinGames { get; set; }

        public int LoseGames { get; set; }

        public int MadeAssumptions { get; set; }

        public int GuessWholeWord { get; set; }
    }
}
    