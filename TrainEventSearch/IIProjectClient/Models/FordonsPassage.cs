using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace IIProjectClient.Models
{
    public class FordonsPassage
    {
        [Display(Name = "Fordon")]
        public Fordon Fordon { get; set; }
        
        [Display(Name = "Plats")]
        public Plats Plats { get; set; }

        [Display(Name = "Tid")]
        public DateTime Tid { get; set; }

        public XElement tillXml()
        {
            return new XElement("FordonsPassage",
                        new XElement("PasseratFordon", this.Fordon),
                        new XElement("PasseradPlats", this.Plats),
                        new XElement("PasseradTid", this.Tid.ToString())
                        );
        }
        public static FordonsPassage frånXml(XElement xmlSvar)
        {
            return new FordonsPassage()
            {
                Fordon = Fordon.frånXml(xmlSvar.Element("Fordon")),
                Plats = Plats.frånXml(xmlSvar.Element("Plats")),
                Tid = Convert.ToDateTime(xmlSvar.Element("Tid").Value),
            };
        }

    }
}