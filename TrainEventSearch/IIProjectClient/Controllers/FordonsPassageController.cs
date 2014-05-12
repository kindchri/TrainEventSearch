using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IIProjectClient.FordonPassageService;
using System.Xml.Linq;
using IIProjectClient.Models;


namespace IIProjectClient.Controllers
{
    public class FordonsPassageController : Controller
    {
        //
        // GET: /Default/
        public ActionResult Index()
        {
            

            FordonPassageServiceClient fpClient = new FordonPassageServiceClient();

            Page model = new Page();


            XElement platser = fpClient.GetAllLocations();

            model.Platser = platser.Element("Platser").Elements("Plats").Select(x => Plats.frånXml(x));

            //Stäng klienten
            fpClient.Close();


            return View(model);
        }

        public ActionResult Search(string Platser, string start, string end, string antaltraffar,int page = 1)
        {
            DateTime startDate;
            DateTime endDate;

            if(!DateTime.TryParse(start, out startDate) || !DateTime.TryParse(end, out endDate))
            {
                return RedirectToAction("Index");
            }

            FordonPassageServiceClient fpClient = new FordonPassageServiceClient();
            
            XElement request = new XElement("Anropsinformation",
                    new XElement("Start", startDate.ToString("o")),
                    new XElement("End", endDate.ToString("o")),
                    new XElement("EPC", Platser),
                    new XElement("Användare", Session["user"].ToString()),
                    new XElement("ResultatPerSida", antaltraffar),
                    new XElement("Sida", page)
                );


            XElement result = fpClient.GetFordonPassage(request);

            //Generera page-modell utifrån xml
            Page model = Page.fromXml(result);

            
            //Hämta locations för select-input
            XElement platser = fpClient.GetAllLocations();
            model.Platser = platser.Element("Platser").Elements("Plats").Select(x => Plats.frånXml(x));


            //Stäng klienten
            fpClient.Close();


            //Sätt nuvarande plats
            model.Plats = Platser;

            //Använd samma vy som index
            return View("Index", model);

        }

        public ActionResult Vehicle()
        {
            return View();   
        }

        public ActionResult FordonsPassager()
        {
            var testelement = new XElement("FordonsPassage",
                                    new XElement("PasseratFordon",
                                        new XElement("FordonsEPC", "urn:epc:id:giai:123456.1847447213244"),
                                        new XElement("FordonsEVN", "217422514471"),
                                        new XElement("Fordonsinnehavare", "Green Cargo AB"),
                                        new XElement("Fordonstyp", "VAGN")),
                                    new XElement("PasseradPlats",
                                        new XElement("PlatsEPC", "urn:epc:id:sgln:735999271.000.13"),
                                        new XElement("PlatsNamn", "Sunderbyns_Sjukhus")),
                                    new XElement("PasseradTid", "2011-03-29T13:25:43.3517370Z"),
                                    new XElement("Underhållsansvarig", "Mattias Mamma"));

            List<FordonsPassage> testlist = new List<FordonsPassage>();
            testlist.Add(FordonsPassage.frånXml(testelement));

            return View(testlist);
        }

        [HttpPost]
        public ActionResult Vehicle(string sökSträng)
        {
            FordonPassageServiceClient fpClient = new FordonPassageServiceClient();

            return View(fpClient.GetVehicleFromEpc(sökSträng));
        }

    }
}
