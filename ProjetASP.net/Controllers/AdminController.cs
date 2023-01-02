using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using ProjetASP.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataPoint = ProjetASP.net.Models.DataPoint;

namespace ProjetASP.net.Controllers
{
    public class AdminController : Controller
    {
        private DataBaseDataContext db = new DataBaseDataContext();

        private bool checkAdmin()
        {
            TempData["Connecter"] = "true";
            int id = Convert.ToInt32(Session["UserId"]);

            var admin = db.Users.Where(r => r.Id == id).FirstOrDefault();
            if (admin == null)
                return false;
            else
                return true;
        }
        public ActionResult Index()
        {
            TempData["Connecter"] = "true";
            if (!checkAdmin())
                return RedirectToAction("Index", "Home");
            int nbrAllUser = db.Users.Count();
            int nbrPropUser = db.Users.Where(r => r.Role.Contains("Proprietaire")).Count();
            int nbrLocataireUser = db.Users.Where(r => r.Role.Contains("Locataire")).Count();
            int nbrReservation = db.Reservations.Count();
            var ListReservation = db.Reservations.GroupBy(r => r.DateDebut.Value.Month).Select(g => new { r = g.Key, count = g.Count() });

            ViewBag.nbrAllUser = nbrAllUser;
            ViewBag.nbrPropUser = nbrPropUser;
            ViewBag.nbrLocataireUser = nbrLocataireUser;
            ViewBag.nbrReservation = nbrReservation;

            Dictionary<int, int> monthsCount = new Dictionary<int, int>();
            foreach (var item in ListReservation)
            {
                monthsCount.Add(item.r, item.count);
            }

            List<DataPoint> dataPoints = new List<DataPoint>();
            dataPoints.Add(new DataPoint("Jan", !monthsCount.ContainsKey(1) ? 0 : monthsCount[1]));
            dataPoints.Add(new DataPoint("Feb", !monthsCount.ContainsKey(2) ? 0 : monthsCount[2]));
            dataPoints.Add(new DataPoint("Mar", !monthsCount.ContainsKey(3) ? 0 : monthsCount[3]));
            dataPoints.Add(new DataPoint("Apr", !monthsCount.ContainsKey(4) ? 0 : monthsCount[4]));
            dataPoints.Add(new DataPoint("Mai", !monthsCount.ContainsKey(5) ? 0 : monthsCount[5]));
            dataPoints.Add(new DataPoint("Jui", !monthsCount.ContainsKey(6) ? 0 : monthsCount[6]));
            dataPoints.Add(new DataPoint("Juil", !monthsCount.ContainsKey(7) ? 0 : monthsCount[7]));
            dataPoints.Add(new DataPoint("Aou", !monthsCount.ContainsKey(8) ? 0 : monthsCount[8]));
            dataPoints.Add(new DataPoint("Sep", !monthsCount.ContainsKey(9) ? 0 : monthsCount[9]));
            dataPoints.Add(new DataPoint("Oct", !monthsCount.ContainsKey(10) ? 0 : monthsCount[10]));
            dataPoints.Add(new DataPoint("Nov", !monthsCount.ContainsKey(11) ? 0 : monthsCount[11]));
            dataPoints.Add(new DataPoint("Dec", !monthsCount.ContainsKey(12) ? 0 : monthsCount[12]));


            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
        public ActionResult Gestion_profil(int id = -1, string op = "")
        {
            TempData["Connecter"] = "true";
            if (!checkAdmin())
                return RedirectToAction("Index", "Home");
            if (id != -1 && !op.Equals(""))
            {
                if (op.Equals("fav"))
                {
                    User us = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    us.Category = 1;
                    db.SubmitChanges();
                }
                if (op.Equals("noir"))
                {
                    User us = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    us.Category = -1;
                    db.SubmitChanges();
                }
                if (op.Equals("sup"))
                {
                    User us = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    db.Users.DeleteOnSubmit(us);
                    db.SubmitChanges();
                }
            }

            return View(db.Users.ToList());
        }

        public ActionResult Gestion_Voiture()
        {
            TempData["Connecter"] = "true";
            if (!checkAdmin())
                return RedirectToAction("Index", "Home");
            var query = (from v in db.Voitures                         //
                         select v).ToList();
            ViewBag.voitures = query;
            return View();

        }
        public ActionResult Delete(int VoitureId)
        {
            TempData["Connecter"] = "true";
            var RES = (from res in db.Reservations
                       where res.Voiture.Equals(VoitureId)
                       select res).ToList();
            RES.ForEach(res => db.Reservations.DeleteOnSubmit(res));
            db.SubmitChanges();
            var query2 = (from voiture in db.Voitures
                          where voiture.Id.Equals(VoitureId)
                          select voiture).First();
            db.Voitures.DeleteOnSubmit(query2);
            db.SubmitChanges();
            return RedirectToAction("Gestion_Voiture");
        }
        public ActionResult Gestion_Resevation()
        {
            TempData["Connecter"] = "true";
            var reservations = from r in db.Reservations
                               join voiture in db.Voitures on r.Voiture equals voiture.Id

                               select new Voiture_info { reservation = r, voiture = voiture };


            ViewBag.Reservations = reservations.ToList();
            return View();
        }
        public ActionResult Gestion_message()
        {
            TempData["Connecter"] = "true";
            if (!checkAdmin())
                return RedirectToAction("Index", "Home");
            var msg = from m in db.Messages
                      select m;
            return View(msg);
        }
    }
}