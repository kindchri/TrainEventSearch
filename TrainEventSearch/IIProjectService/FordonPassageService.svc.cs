using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using IIProjectService.IIService;
using System.Web.Hosting;


namespace IIProjectService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class FordonPassageService : IFordonPassageService
    {
        //AppDatafolder
        string appDataFolder = HostingEnvironment.MapPath("/App_Data/");


        /// <summary>
        /// End point to get FordonPasssage
        /// </summary>
        /// <param name="request">XElement containing Start, End and location EPC</param>
        /// <returns>Fordonspassage Element</returns>
        public XElement GetFordonPassage(XElement request)
        {
            //Instansiera klinter för services
            EpcisEventServiceClient eventClient = new EpcisEventServiceClient();
            NamingServiceClient namingClient = new NamingServiceClient();



            //Kolla så att requestens element är valida
            XElement response = GetResponseInfoAndCheckRequest(request);
            if(response.Element("Svarsinformation").Element("Svarskod").Value != "OK")
            {
                return response;
            }

            //Inputvariabler definieras
            DateTime start = (DateTime)request.Element("Start");
            DateTime end = (DateTime)request.Element("End");
            string epc = (string)request.Element("EPC");
            int sida;
            int resultatPerSida;

            if(request.Elements("Sida").Any())
            {
                sida = (int)request.Element("Sida");
            }
            else
            {
                sida = 1;
            }

            if(request.Elements("ResultatPerSida").Any())
            {
                resultatPerSida = (int)request.Element("ResultatPerSida");
            }
            else
            {
                resultatPerSida = 10;
            }



            //Hämta events
            XNamespace ns = "urn:epcglobal:epcis:xsd:1";
            IEnumerable<XElement> result = eventClient.GetEvents(start, end, epc);
            eventClient.Close();
            List<XElement> objEvents = new List<XElement>();

            //Hämta ut OjbectEventelement
            foreach (XElement epcisdoc in result)
            {
                objEvents.AddRange(epcisdoc.Elements("EPCISBody").Elements("EventList").Elements("ObjectEvent"));
            }
            

            //Hämta antalet träffar
            int hits = objEvents.Count();

            //Skip och take för pagination
            int skip = (sida - 1) * resultatPerSida;
            objEvents = objEvents.Skip(skip).Take(resultatPerSida).ToList();


            XElement fordonsPassager = new XElement("FordonsPassager");
            //int antalSidor = (int)Math.Ceiling((decimal)((int)hits / resultatPerSida));
            int antalSidor;
            if(hits % resultatPerSida == 0)
            {
                antalSidor = hits / resultatPerSida;
            }
            else
            {
                antalSidor = (int)Math.Floor((double)hits / (double)resultatPerSida) + 1;
            }
            //Hämta alla locations, går snabbare än att hämta varje
           // XElement locations = namingClient.GetAllLocations().Element("Locations");
            XElement location = GetLocationFromEpc(epc);
            namingClient.Close();


            //Iterera igenom events för att populera resposne
            foreach(XElement objEvent in objEvents)
            {
                //Hämta location data
                //XElement location = locations.Elements("Location").Where(l => l.Element("Epc").Value == (objEvent.Element("readPoint").Element("id").Value)).First();


                //Skapa fordonspassageelementet
                fordonsPassager.Add(new XElement("FordonsPassage",
                                GetVehicleFromEpc(objEvent.Element("epcList").Element("epc").Value),
                                location,
                                new XElement("Tid", objEvent.Element("eventTime").Value)
                        ));
            }


            //Lägg till fordonspassage i responseobjektet
            response.Add(fordonsPassager);
            response.Element("Svarsinformation").Add(
                    new XElement("Sida", sida),
                    new XElement("Träffar", hits),
                    new XElement("AntalSidor", antalSidor),
                    new XElement("ResultatPerSida", resultatPerSida)
                );

            return response;

        }

        public XElement GetResponseInfoAndCheckRequest(XElement request)
        {
            //User
            DateTime start;
            DateTime end;
            int resultatPerSida;
            int sida;

            XElement response = new XElement("Svarsinformation",
                    new XElement("Svarskod", "OK"),
                    new XElement("Meddelanden"),
                    new XElement("Tjänsteansvarig", "Grupp 11"),
                    new XElement("Applikation", "SupaDupaHyphyRailwayInfo 0.1"),
                    new XElement("Svarstid", DateTime.Now.ToString("o"))
                );

            //Kolla så att en användare är skickad
            if (request.Elements("Användare").Any())
            {
                //Kolla så att användaren är behörig
                if (!Authorize(request))
                {
                    response.Element("Svarskod").Value = "AUTHORIZATION ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Användaren har ej tillgång till tjänsten"));
                    return response;
                }
            }
            else
            {
                response.Element("Svarskod").Value = "INTERNAL ERROR";
                response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange en användare av tjänsten"));
            }

            //Kontrollera att ett startdatum skickats samt har korrekt format
            try
            {
                start = (DateTime)request.Element("Start");
            }
            catch (Exception ex)
            {
                //Startdatum saknas
                if(ex is ArgumentNullException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange ett startdatum"));
                }
                //Felaktigt format
                else if (ex is FormatException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange ett startdatum i korrekt format"));
                }
                else
                {
                    throw;
                }
            }


            //Kontrollera att ett startdatum skickats samt har korrekt format
            try
            {
                end = (DateTime)request.Element("End");
            }
            catch (Exception ex)
            {
                //Startdatum saknas
                if (ex is ArgumentNullException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange ett slutdatum"));
                }
                //Felaktigt format
                else if (ex is FormatException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange ett slutdatum i korrekt format"));
                }
                else
                {
                    throw;
                }
            }

            //Kontrollera att en platsEPC skicakts
            if(!request.Elements("EPC").Any() || request.Element("EPC").IsEmpty)
            {
                response.Element("Svarskod").Value = "INTERNAL ERROR";
                response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange en EPC för plats"));
            }

            //Kontrollera om ResultatPerSida är i fel format
            try
            {
                resultatPerSida = (int)request.Element("ResultatPerSida");
            }
            catch(Exception ex)
            {
                if(ex is FormatException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange resultat per sida i korrekt format"));
                }
                else if (!(ex is ArgumentNullException))
                {
                    throw;
                }
            }

            //Kontrollera om Sida är i fel format
            try
            {
                sida = (int)request.Element("Sida");
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    response.Element("Svarskod").Value = "INTERNAL ERROR";
                    response.Element("Meddelanden").Add(new XElement("Meddelande", "Var god ange sida i korrekt format"));
                }
                else if (!(ex is ArgumentNullException))
                {
                    throw;
                }
            }

            XElement result =  new XElement("Response",
                                    request,
                                    response
                                );

            return result;
        }

        
        /// <summary>
        /// Endpoint to get all locations
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public XElement GetAllLocations()
        {
            NamingServiceClient namingClient = new NamingServiceClient();
            XElement result = namingClient.GetAllLocations();
            namingClient.Close();

            XElement response = GetResponseInfo(new XElement("Response"), "OK", "OK");
            
            response.Add(new XElement("Platser",
                result.Element("Locations").Elements("Location").Select(x => new XElement("Plats",
                    new XElement("PlatsEPC", x.Element("Epc").Value),
                    new XElement("PlatsNamn", x.Element("Name").Value)
                    ))
                ));
            
            return response;
        }

        XElement GetResponseInfo(XElement request, string responseCode, string responseMsg)
        {
            return new XElement("Response",
                request,
                new XElement("Svarsinformation",
                    new XElement("Svarskod", responseCode),
                    new XElement("Meddelande", responseMsg),
                    new XElement("Tjänsteansvarig", "Grupp 11"),
                    new XElement("Applikation", "SupaDupaHyphyRailwayInfo 0.1"),
                    new XElement("Svarstid", DateTime.Now.ToString("o"))
                ));
        }

        /// <summary>
        /// Endpoint to search vehicles on EPC
        /// </summary>
        /// <param name="epc"></param>
        /// <returns>An XElement containing vehicle information formatted to fit the Fordon class</returns>
        public XElement GetVehicleFromEpc(string epc)
        {
            NamingServiceClient namingClient = new NamingServiceClient();

            XElement specificVehicle = namingClient.GetVehicle(epc);
            namingClient.Close();
            if (specificVehicle.Element("ResponseData").Elements("ResponseMessage").Any())
            {
                return new XElement("Fordon",
                                new XElement("FordonsEPC", epc));

            }

            XElement godkännande;

            //Skapa godkännandeelement
            try
            {
                 godkännande = new XElement("Godkännande",
                    new XElement("GiltigtFrom", specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Element("Godkannande").Element("GiltigtFrom").Value)
                );
            }
            catch(NullReferenceException)
            {
                godkännande = new XElement("Godkännande",
                        new XElement("GiltigtFrom", "NoVehicleException")
                    );
            }

            //Kontrollera om giltigt godkännande
           
            if (specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Elements("Godkannanade").Elements("GiltigtTom").Any())
            {
                godkännande.Add(
                        new XElement("GiltigtTom", specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Element("Godkannande").Element("GiltigtTom").Value),
                        new XElement("Godkänd", "Ej godkänd")
                    );
            }
            else
            {
                godkännande.Add(new XElement("Godkänd", "Godkänd"));
            
            }
            XElement specificVehicleRelevantData = new XElement("Fordon",
                                                        new XElement("FordonsEPC", epc),
                                                        new XElement("FordonsEVN", (string)specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Element("Fordonsnummer")),
                                                        new XElement("Fordonsinnehavare", (string)specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Element("Fordonsinnehavare").Element("Foretag")),
                                                        new XElement("Underhållsansvarig", (string)specificVehicle.Element("Fordonsindivider").Element("FordonsIndivid").Element("UnderhallsansvarigtForetag").Element("Foretag")),
                                                        new XElement("Fordonstyp", (string)specificVehicle.Element("FordonsTyp").Element("FordonskategoriKodFullVardeSE")),
                                                        godkännande
                );

            return specificVehicleRelevantData;
        }

        public XElement GetLocationFromEpc(string epc)
        {
            NamingServiceClient namingClient = new NamingServiceClient();
            XElement specificLocation = namingClient.GetLocation(epc);

            XElement specificLocationRelevantData = new XElement("Plats",
                                                        new XElement("PlatsEPC", (string)specificLocation.Element("Location").Element("Epc")),
                                                        new XElement("PlatsNamn", (string)specificLocation.Element("Location").Element("Name"))

                                                                );
            return specificLocationRelevantData;

        }

        bool Authorize(XElement request)
        {
            XElement users = XElement.Load(appDataFolder + "Users.xml");

            if(!users.Elements().Where(u => u.Value == request.Element("Användare").Value).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public bool AuthorizeUser(string username)
        {
            XElement users = XElement.Load(appDataFolder + "Users.xml");

            if(users.Elements().Where(u => (string)u.Value == username).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
