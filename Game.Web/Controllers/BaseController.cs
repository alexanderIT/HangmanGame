using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.BL;
using Game.DAL.EFDbContext;

namespace Game.Web.Controllers
{
    public class BaseController : Controller
    {
        protected GameManager GameManager;

        protected NewUserManager UserManager;

        public BaseController(GameManager gameManager, NewUserManager userManager)
        {
            GameManager = gameManager;
            UserManager = userManager;
        }

        public BaseController()
            : this(new GameManager(), new NewUserManager())
        {

        }
    }
}