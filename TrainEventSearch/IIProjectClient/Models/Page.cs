using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IIProjectClient.Models
{
    public class Page
    {
        public int NuvarandeSida { get; set; }
        public int TotaltAntalSidor { get; set; }
        public int FordonsPassagerPerSida { get; set; }
        public int? TotaltAntalPassager { get; set; }
        public string Plats { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IEnumerable<FordonsPassage> FordonsPassager { get; set; }
        public IEnumerable<Plats> Platser { get; set; }

        public bool Föregåendesida
        {
            get
            {
                if (NuvarandeSida == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool Nästasida
        {
            get
            {
                if(NuvarandeSida + 1 < TotaltAntalSidor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public void NästaSida()
        {
            if((NuvarandeSida + 1) <= TotaltAntalSidor)
                NuvarandeSida++;
        }

        public void FöregåendeSida()
        {
            if ((NuvarandeSida - 1) >= 1)
                NuvarandeSida--;
        }
        
        public static Page fromXml(XElement xml)
        {
            //FXIAAA
            return new Page()
            {
                NuvarandeSida = (int)xml.Element("Anropsinformation").Element("Sida"),
                TotaltAntalPassager = (int)xml.Element("Svarsinformation").Element("Träffar"),
                TotaltAntalSidor = (int)xml.Element("Svarsinformation").Element("AntalSidor"),
                FordonsPassager = xml.Element("FordonsPassager").Elements("FordonsPassage").Select(x => FordonsPassage.frånXml(x)),
                Plats = (string)xml.Element("Anropsinformation").Element("EPC"),
                Start = (DateTime)xml.Element("Anropsinformation").Element("Start"),
                End = (DateTime)xml.Element("Anropsinformation").Element("End")
            };
        }



    }
}