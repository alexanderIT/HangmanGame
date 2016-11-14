using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.BL;
using Game.DAL.DataObject;
using Game.Web.Models;
using Microsoft.AspNet.Identity;

namespace Game.Web.Controllers
{
    public class GameController : BaseController
    {
        private readonly IList<PlayerViewModel> _userList;

        public GameController()
        {
            _userList = GetAllPlayers();
        }

        private ApplicationUser GetCurrentUser()
        {
            var userId = this.User.Identity.GetUserId();
            return UserManager.FindById(userId);
        }

        [HttpGet]
        public ActionResult GameBegin(int typeOfModelId)
        {
            GameManager.GetStartSecretWord(typeOfModelId);

            var model = new GameViewModel
            {
                FullWord = Word.FullWord,
                PartialWord = Word.PartialWord,
                UsedLetters = Word.UsedLetters,
                Guesses = GameManager.guesses,
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult GuessLetter(string letter)
        {
            var model = new GameViewModel
            {
                FullWord = Word.FullWord,
                PartialWord = Word.PartialWord,
                UsedLetters = Word.UsedLetters,
                Guesses = GameManager.guesses
            };

            if (string.IsNullOrEmpty(letter) && GameManager.guesses > 0)
            {

                var user = GetCurrentUser();
                user.MadeAssumptions = user.MadeAssumptions + 1;
                return PartialView("_SecretWord", model);
            }


            if (GameManager.TryGuessWholeWord(letter))
            {
                var user = GetCurrentUser();
                user.GuessWholeWord = user.GuessWholeWord + 1;
                return RedirectToAction("Winner");
            }
            else
            {
                var user = GetCurrentUser();
                user.LoseGames = user.LoseGames + 1;
                GameManager.guesses = 0;
            }
           
            GameManager.SearchLetter(letter);

            model.Guesses = GameManager.guesses;

            if (GameManager.guesses == 0)
            {
                return RedirectToAction("GameOver");
            }
            if (GameManager.IfTheWordIsComplete())
            {
                return RedirectToAction("Winner");
            }
            return PartialView("_SecretWord", model);

        }

        [HttpGet]
        public ActionResult DisplayPlayer()
        {
            var topPlayers = _userList.OrderByDescending(x => x.Score).ToList();

            return PartialView("_DisplayPlayer", topPlayers);
        }

        public ActionResult Winner()
        {
            var user = GetCurrentUser();
            user.Score = user.Score + 1;
            user.WinGames = user.WinGames + 1;
            UserManager.Update(user);


            return PartialView("_Winner", Word.PartialWord);
        }

        public ActionResult GameOver()
        {
            var user = GetCurrentUser();
            user.LoseGames = user.LoseGames + 1;
            return PartialView("_GameOver", Word.FullWord);
        }

        private List<PlayerViewModel> GetAllPlayers()
        {
            var users = UserManager.GetAllUsers();

            return users.Select(item => new PlayerViewModel()
            {
                Name = item.UserName,
                Score = item.Score,
                WinGames = item.WinGames,
                LoseGames = item.LoseGames,
                MadeAssumptions = item.MadeAssumptions
            }).ToList();
        }
    }
}