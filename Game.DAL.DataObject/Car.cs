using System.Collections.Generic;

namespace Game.DAL.DataObject
{
    public class Car : IPlayObject
    {
        public static HashSet<string> IgnoreList = new HashSet<string> { nameof(Id) };

        public int Id { get; set; }

        public string Model { get; set; }

        public string Tooltip { get; set; }

    }
}
