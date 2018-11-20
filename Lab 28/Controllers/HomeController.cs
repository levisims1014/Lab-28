using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab28.Models;
using Newtonsoft.Json.Linq;

namespace Lab28.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateDeck()
        {
            //declare deck id
            string deck_id;

            //make our request
            HttpWebRequest request = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");

            //following line is optional, lets you use dummy information
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0";

            //make your response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                //get response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                //read response stream as string
                string output = reader.ReadToEnd();

                //convert response to JSon
                JObject Jparser = JObject.Parse(output);


                //if TempData is empty
                if (TempData["deck_id"] == null)
                {
                    //get the deck ID from the JSON and convert to string
                    TempData["deck_id"] = Jparser["deck_id"];
                    deck_id = Jparser["deck_id"].ToString();
                }
                // otherwise set the new deck id
                else
                {
                    //convert the deck ID to string
                    deck_id = TempData["deck_id"].ToString();
                }
                // put the deck id in
                ViewBag.Deck = deck_id;
                reader.Close();

            }

            return View("Index");
        }
        public ActionResult DrawCards(string deck_id)
        {
            //Make a cookie to store the temporary data
            HttpCookie cookie;

            //If Request.Cookies = Null then make a new cookie
            //If the cookie doesn't exist, make it and add it to the user's browser
            if (Request.Cookies["deck_id"] == null)
            {
                cookie = new HttpCookie("deck_id");
                cookie.Value = deck_id;
            }
            //anything else Request Cookie
            else
            {
                cookie = Request.Cookies["deck_id"];
                cookie.Value = deck_id;
            }

            deck_id = cookie.Value.ToString();
            Response.Cookies.Add(cookie);

            //Make the request
            HttpWebRequest request = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/" + deck_id + "/draw/?count=5");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Check to see if the response is ok
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string output = reader.ReadToEnd();

                //turn it into JSON
                JObject Jparser = JObject.Parse(output);

                //Make an array of Card objects and make new Card objects or each card in the JSON
                Card[] cards = new Card[5];
                int i = 0;
                foreach (var x in Jparser["cards"])
                {
                    //Make a new Card object for each card in the JSON and add it to the array
                    cards[i] = new Card(x["image"].ToString(), x["value"].ToString(), x["suit"].ToString());
                    i++;
                }
                //Send the cards to the View
                ViewBag.CardsInHand = cards;
                reader.Close();
                response.Close();
                return View("Index");

            }
            return View("Index");
        }
    }
}