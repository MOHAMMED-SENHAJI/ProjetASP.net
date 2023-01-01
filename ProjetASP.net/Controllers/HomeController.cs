using ProjetASP.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetASP.net.Controllers
{

    public class HomeController : Controller
    {
        private DataBaseDataContext db = new DataBaseDataContext();
        public ActionResult Index()
        {
            if (Session["UserRole"] != null)
            {
                TempData["Connecter"] = "true";
            }

            else
            {
                TempData["Connecter"] = "false";
            }

            var listVoiture = (from v in db.Voitures
                               join p in db.Users on v.Proprietaire equals p.Id
                               where v.Offre == 1
                               select new Voiture_info { voiture = v, user = p }).Take(8);
            return View(listVoiture.ToList());

        }
        public ActionResult Contact()
        {
            if (Session["UserRole"] != null)
            {
                TempData["Connecter"] = "true";
            }

            else
            {
                TempData["Connecter"] = "false";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string name, string em, string msg)
        {
            if (Session["UserRole"] != null)
            {
                TempData["Connecter"] = "true";
            }

            else
            {
                TempData["Connecter"] = "false";
            }
            Message message = new Message
            {
                Nom = name,
                Email = em,
                Description = msg
            };
            db.Messages.InsertOnSubmit(message);
            db.SubmitChanges();
            return View();
        }
        public ActionResult About()
        {
            if (Session["UserRole"] != null)
            {
                TempData["Connecter"] = "true";
            }

            else
            {
                TempData["Connecter"] = "false";
            }
            return View();
        }

    }
}