using System.Collections.Generic;

namespace Game.DAL.DataObject
{
    public class Animal : IPlayObject
    {
        public static HashSet<string> IgnoreList = new HashSet<string> { nameof(Id) };

        public int Id { get; set; }

        public string Breed { get; set; }

        public string Tooltip { get; set; }

        public Animal Create()
        {
            return new Animal();
        }
    }
}
