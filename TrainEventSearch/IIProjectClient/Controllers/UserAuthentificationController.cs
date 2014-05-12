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
    public class UserAuthentificationController : Controller
    {
        
        //
        // GET: /UserAuthentification/
       
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            FordonPassageServiceClient service = new FordonPassageServiceClient();
            if (service.AuthorizeUser(user))
            {
                Session["user"] = user;
                Session["loginerror"] = null;
            }
            else
            {
                Session["loginerror"] = "Felaktigt användarnamn";
            }
            return RedirectToAction("Index", "FordonsPassage");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            
            Session["user"] = null;
            
            return RedirectToAction("Index", "FordonsPassage");
        }

    }
}
