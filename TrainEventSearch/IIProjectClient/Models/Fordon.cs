using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace IIProjectClient.Models
{
    public class Fordon
    {
        [Display(Name = "FordonsEPC")]
        public string FordonsEPC { get; set; }

        [Display(Name = "FordonsEVN")]
        public string FordonsEVN { get; set; }

        [Display(Name = "Fordonsinnehavare")]
        public string Fordonsinnehavare { get; set; }

        [Display(Name = "Fordonstyp")]
        public string Fordonstyp { get; set; }

        [Display(Name = "UnderhållsAnsvarig")]
        public string UnderhållsAnsvarig { get; set; }

        [Display(Name = "Godkännande")]
        public FordonsGodkännande Godkännande { get; set; }

        public XElement tillXml()
        {
            return new XElement("Fordon",
                        new XElement("FordonsEPC", this.FordonsEPC),
                        new XElement("FordonsEVN", this.FordonsEVN),
                        new XElement("Fordonsinnehavare", this.Fordonsinnehavare),
                        new XElement("Fordonstyp", this.Fordonstyp),   
                        new XElement("Underhållsansvarig", this.UnderhållsAnsvarig),
                        new XElement("Godkännande", this.Godkännande)
                        );
        }
        public static Fordon frånXml(XElement xmlSvar)
        {
            return new Fordon()
            {
                FordonsEPC = (string)xmlSvar.Element("FordonsEPC"),
                FordonsEVN = (string)xmlSvar.Element("FordonsEVN"),
                Fordonsinnehavare = (string)xmlSvar.Element("Fordonsinnehavare"),
                Fordonstyp = (string)xmlSvar.Element("Fordonstyp"),    
                UnderhållsAnsvarig = (string)xmlSvar.Element("Underhållsansvarig"),
                Godkännande = FordonsGodkännande.frånXml(xmlSvar.Element("Godkännande"))
            };
        }
    }
}