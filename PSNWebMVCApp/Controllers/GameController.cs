using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSNWebMVCApp.Models;
using System.Xml;
using System.Net;

namespace PSNWebMVCApp.Controllers
{
    public class GameController : Controller
    {
        private GameContext db = new GameContext();

        //
        // GET: /Game/

        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        //
        // GET: /Game/Details/5

        public ActionResult Details(int id = 0)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        //
        // GET: /Game/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult UpdateGames()
        {
            Chilkat.Http http = new Chilkat.Http();

            bool success;
            success = http.UnlockComponent("30-day trial");
            if (success != true)
            {
                return View();
            }
            for (int i = 1; i < 79; i++)
            {
                string html;
                string url = String.Format("http://psntrophyleaders.com/games/{0}?sort=title-all-asc", i.ToString());
                html = http.QuickGetStr(url);
                if (html == null)
                {
                    //TextBox1.Text += http.LastErrorText + "\r\n";
                    return View();
                }

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                success = htmlToXml.UnlockComponent("30-day trial");
                if (success != true)
                {
                    return View();
                }

                htmlToXml.XmlCharset = "utf-8";
                htmlToXml.Html = html;

                string xml;
                xml = htmlToXml.ToXml();

                //success = htmlToXml.WriteStringToFile(xml, "C:/temp/out.xml", "utf-8");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                //XmlNodeList gamesList = doc.SelectNodes("//*[@class='gameMain']");
                XmlNodeList titleNodeList = doc.SelectNodes("//*[@class='gameTitle']");
                //XmlNodeList platformNodeList = doc.SelectNodes("//*[contains(concat(' ', @class, ' '), ' platformlabel ')]");
                XmlNodeList platformNodeList = doc.SelectNodes("//*[@class='gameHeadSpace']");
                XmlNodeList imageStringNodeList = doc.SelectNodes("//*[@class='game-image-cell']");
                XmlNodeList difficultyNodeList = doc.SelectNodes("//*[@class='difficultyText']");
                XmlNodeList difficultyPointsNodeList = doc.SelectNodes("//*[@class='difficultyPoints']");
                XmlNodeList completedNodeList = doc.SelectNodes("//*[@class='numcomplete']");
                XmlNodeList ownedNodeList = doc.SelectNodes("//*[@class='num_played']");
                XmlNodeList linkNodeList = doc.SelectNodes("//*[@class='user-select']");
                int count = 0;
                foreach (XmlNode titleNode in titleNodeList)
                {
                    Game newGame = new Game();
                    //Title
                    newGame.Title = titleNodeList[count].InnerText;
                    //Platform
                    newGame.Platform = platformNodeList[count].InnerText;
                    //ImageString
                    newGame.ImageString = imageStringNodeList[count].Attributes["src"].Value;
                    //Difficulty
                    newGame.Difficulty = difficultyNodeList[count].InnerText;
                    //DifficultyPoints
                    newGame.DifficultyPoints = difficultyPointsNodeList[count].InnerText;
                    //Completed
                    newGame.Completed = completedNodeList[count].InnerText;
                    //Owned
                    newGame.Owned = ownedNodeList[count].InnerText;
                    //Link
                    newGame.Link = linkNodeList[count*2].Attributes["href"].Value;

                    Create(newGame);
                    count++;
                }
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Game/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        //
        // GET: /Game/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        //
        // POST: /Game/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        //
        // GET: /Game/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        //
        // POST: /Game/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}