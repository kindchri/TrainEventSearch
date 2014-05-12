using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IIProjectClient.Models
{
    public class Användare
    {
        public int AnvändarId { get; set; }
        public string Användarnamn { get; set; }
        public string Lösenord { get; set; }



        public XElement tillXml()
        {
            return new XElement("Användare",
                        new XElement("AnvändarId", this.AnvändarId),
                        new XElement("Användarnamn", this.Användarnamn),
                        new XElement("Lösenord", this.Lösenord)
                        );
        }

        public static Användare frånXml(XElement xmlSvar)
        {
            return new Användare()
            {
                AnvändarId = (int)xmlSvar.Element("AnvändarId"),
                Användarnamn = (string)xmlSvar.Element("Användarnamn"),
                Lösenord = (string)xmlSvar.Element("Lösenord")
            };
        }
    }
}