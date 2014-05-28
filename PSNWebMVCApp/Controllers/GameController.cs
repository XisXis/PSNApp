using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSNWebMVCApp.Models;
using System.Xml;

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

            string html;
            //html = http.QuickGetStr("http://www.exophase.com/platform/ps3-games/");
            html = http.QuickGetStr("http://psntrophyleaders.com/games/1?sort=title-all-asc");
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

            success = htmlToXml.WriteStringToFile(xml, "C:/temp/out.xml", "utf-8");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList gamesTitleList = doc.SelectNodes("//*[@class='gameTitle']");

            return View();
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