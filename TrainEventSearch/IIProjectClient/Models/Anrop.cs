using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IIProjectClient.Models
{
    public class Anrop
    {
        public int AnropsId { get; set; }
        public DateTime Starttid { get; set; }
        public DateTime Sluttid { get; set; }
        public Plats SöktPlats { get; set; }

        public XElement tillXml()
        {
            return new XElement("Anrop",
                        new XElement("AnropsId", this.AnropsId),
                        new XElement("Starttid", this.Starttid.ToString()),
                        new XElement("Sluttid", this.Sluttid.ToString()),
                        new XElement("Söktplats", this.SöktPlats)
                        );
        }

        public static Anrop frånXml(XElement xmlSvar)
        {
            return new Anrop()
            {
                AnropsId = (int)xmlSvar.Element("AnropsId"),
                Starttid = Convert.ToDateTime(xmlSvar.Element("Starttid")),
                Sluttid = Convert.ToDateTime(xmlSvar.Element("Sluttid")),
                SöktPlats = Plats.frånXml(xmlSvar.Element("Söktplats"))
            };
        }
        
    }

}