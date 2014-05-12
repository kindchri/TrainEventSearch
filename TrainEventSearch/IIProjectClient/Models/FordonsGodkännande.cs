using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IIProjectClient.Models
{
    public class FordonsGodkännande
    {
        public bool Godkänt { get; set; }
        public DateTime? GiltigtTom { get; set; }
        public DateTime? GiltigtFrom { get; set; }

        public XElement tillXml()
        {
            return new XElement("FordonsGodkännande",
                        new XElement("Godkänt", this.Godkänt.ToString()),
                        new XElement("GiltigtTom", this.GiltigtTom.ToString()),
                        new XElement("GiltigtFrom", this.GiltigtFrom.ToString())
                        );
        }
        public static FordonsGodkännande frånXml(XElement xmlSvar)
        {
            FordonsGodkännande godkännande = new FordonsGodkännande();
            try
            {
                if (xmlSvar.Elements("GiltigtFrom").Any())
                {
                    godkännande.GiltigtFrom = Convert.ToDateTime(xmlSvar.Element("GiltigtFrom").Value);
                }

                if (xmlSvar.Elements("GiltigtTom").Any())
                {
                    godkännande.GiltigtTom = Convert.ToDateTime(xmlSvar.Element("GiltigtTom").Value);
                    godkännande.Godkänt = false;

                }
                else
                {
                    godkännande.GiltigtTom = null;
                    godkännande.Godkänt = true;
                }
            }
            catch (NullReferenceException)
            {
                godkännande.GiltigtFrom = null;
                godkännande.GiltigtTom = null;
                godkännande.Godkänt = false;
            }

            return godkännande;


        }
    }
}