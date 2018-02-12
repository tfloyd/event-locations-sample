using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Data;
using EventPlaces.Models;
using System.Xml;


namespace EventPlaces.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {   
            //Google API key
            //AIzaSyDV5ZpiGxHDWDVMBGXVeMQxL6NEjUY5t5Q

            Console.WriteLine("Started Home");

            return View();
        }

        // GetNextEvent: 
        // Gets the next event or else it gets the first 
        // returns JSON result
        public ActionResult GetNextEvent(int currentEventId = -1)
        {
            
            XmlDocument doc = new XmlDocument();

            //Load file else return None string
            try { 
                doc.Load("Content/Developer Evaluation Events.xml");
            }
            catch (System.IO.FileNotFoundException)
            {   
                return Content("None");
            }

            XmlNode currNode;

            //Query for node using xpath
            currNode = doc.SelectSingleNode("//events/event[@id='" + 
                                            currentEventId + "']");

            //If event not found or is the last event then get the first
            if(currNode == null || currNode == doc.DocumentElement.LastChild){

                currNode = doc.DocumentElement.FirstChild;

            }else{ //Get the next event

                currNode = currNode.NextSibling;

            }

            //Cast object with values
            var e = new Event { 
                Id = currNode.Attributes["id"].Value,
                Name = currNode.Attributes["name"].Value,
                Date = currNode.Attributes["date"].Value,
                Address1 = currNode.FirstChild.Attributes["address1"].Value,
                Address2 = currNode.FirstChild.Attributes["address2"].Value,
                Suburb = currNode.FirstChild.Attributes["suburb"].Value,
                State = currNode.FirstChild.Attributes["state"].Value,
                Country = currNode.FirstChild.Attributes["country"].Value
            };

            //Return as JSON
            return Json(e, JsonRequestBehavior.AllowGet);

        }

    }
}
