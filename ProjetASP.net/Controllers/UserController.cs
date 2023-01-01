using ProjetASP.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetASP.net.Controllers
{
    public class UserController : Controller
    {
        private DataBaseDataContext db = new DataBaseDataContext();



        public ActionResult Index()
        {
            TempData["Connecter"] = "true";

            if (Session["UserId"] != null)
                return RedirectToAction("UserInfo");
            else
                return RedirectToAction("Index", "Home");


        }
        public ActionResult UserInfo()
        {
            TempData["Connecter"] = "true";

            return View(getUser());
        }
        [HttpPost]
        public ActionResult UpdateUserInfo(string nom, string email, string telephone)
        {
            TempData["Connecter"] = "true";
            User user = getUser();
            user.Name = nom;
            user.Email = email;
            user.Phone = telephone;
            db.SubmitChanges();
            return RedirectToAction("UserInfo");
        }
        public ActionResult Historique()
        {
            TempData["Connecter"] = "true";
            User user = getUser();

            var histo_info = from c in db.Reservations
                             where c.Locataire == Convert.ToInt32(Session["UserId"])
                             select new Voiture_info
                             {
                                 voiture = c.Voiture1,
                                 user = c.Voiture1.User,
                                 reservation = c
                             };
            return View(histo_info);
        }

        private User getUser()
        {
            TempData["Connecter"] = "true";
            User user = (from u in db.Users
                         where u.Id == Convert.ToInt32(Session["UserId"])
                         select u).FirstOrDefault();
            return user;
        }

        private Voiture getVoiture(int id)
        {
            TempData["Connecter"] = "true";
            Voiture user = (from u in db.Voitures
                            where u.Id == id
                            select u).FirstOrDefault();
            return user;
        }
        private User getProp(int id)
        {
            TempData["Connecter"] = "true";
            User user = (from u in db.Users
                         where u.Id == id
                         select u).FirstOrDefault();
            return user;
        }



    }
}

