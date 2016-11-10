using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.BL;

namespace Game.Web.Models
{
    public class HomeViewModel
    {
        [Display(Name = "Type of word")]
        public int TypeOfWordId { get; set; }

        public IEnumerable<SelectListItem> TypeOfWord { get; set; }
    }
}