using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Game.Web.Models
{
    public class PlayerViewModel
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public int WinGames { get; set; }

        public int LoseGames { get; set; }

        public int MadeAssumptions { get; set; }

        public int GuessWholeWord { get; set; }

    }
}