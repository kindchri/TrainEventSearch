using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace IIProjectClient.Models
{
    public class Plats
    {
        [Display(Name = "PlatsEPC")]
        public string PlatsEPC { get; set; }

        [Display(Name = "PlatsNamn")]
        public string PlatsNamn { get; set; }

        public XElement tillXml()
        {
            return new XElement("Plats",
                        new XElement("PlatsEPC", this.PlatsEPC),
                        new XElement("PlatsNamn", this.PlatsNamn)
                        );
        }

        public static Plats frånXml(XElement xmlSvar)
        {
            return new Plats()
            {
                PlatsEPC = (string)xmlSvar.Element("PlatsEPC"),
                PlatsNamn = (string)xmlSvar.Element("PlatsNamn"),
            };
        }
    }
}