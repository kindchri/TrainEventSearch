using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IIProjectClient.Models
{
    public class AnropsSvar
    {
        public int AnropsSvarId { get; set; }
        public int SvarsId { get; set; }
        public DateTime SvarsDatum { get; set; }
        public string Meddelande { get; set; }
        public string TjänsteAnsvarig { get; set; }
        public string ApplikationsNamn { get; set; }
        public string ApplikationsVersion { get; set; }

        public XElement tillXml()
        {
            return new XElement("AnropsSvar",
                        new XElement("AnropsId", this.AnropsSvarId),
                        new XElement("SvarsId", this.SvarsId),
                        new XElement("SvarsDatum", Convert.ToString(this.SvarsDatum),
                        new XElement("Meddelande", this.Meddelande),
                        new XElement("TjänsteAnsvarig", this.TjänsteAnsvarig),
                        new XElement("ApplikationsNamn", this.ApplikationsNamn),
                        new XElement("ApplikationsVersion", this.ApplikationsVersion)
                        ));
        }

        public static AnropsSvar frånXml(XElement xmlSvar)
        {
            return new AnropsSvar()
            {
                AnropsSvarId = (int)xmlSvar.Element("AnropsId"),
                SvarsId = (int)xmlSvar.Element("SvarsId"),
                SvarsDatum = Convert.ToDateTime(xmlSvar.Element("SvarsDatum")),
                Meddelande = (string)xmlSvar.Element("Meddelande"),
                TjänsteAnsvarig = (string)xmlSvar.Element("TjänsteAnsvarig"),
                ApplikationsNamn = (string)xmlSvar.Element("ApplikationsNamn"),
                ApplikationsVersion = (string)xmlSvar.Element("ApplikationsVersion")
            };
        }

    }
}