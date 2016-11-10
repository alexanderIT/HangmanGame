
using System.Collections.Generic;
using Game.BL;


namespace Game.Web.Models
{
    public class GameViewModel
    {
        public int Guesses { get; set; }

        public TypeOfWord TypeOfWord { get; set; }

        public char[] FullWord { get; set; }

        public char[] PartialWord { get; set; }

        public char[] UsedLetters { get; set; }

        public List<PlayerViewModel> UserList { get; set; }
    }
}