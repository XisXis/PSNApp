using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PSNWebMVCApp.Models;
using System.Threading.Tasks;

namespace PSNWebMVCApp.Controllers
{
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login loginInfo)
        {
            string userName = loginInfo.Username;
            string password = loginInfo.Password;
            var myWebClient = new WebClient();
            string urlString = String.Format("http://localhost:3000/PSN/Login/{0}/{1}", userName, password);
            var resultBytes = await myWebClient.DownloadDataTaskAsync(urlString);
            string resultStr = System.Text.Encoding.UTF8.GetString(resultBytes);
            if (resultStr.Contains("SUCCESS"))
            {
                ViewBag.UserName = userName;
                return RedirectToAction("Index", "Game");
            }
            else
            {
                return View("Index");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMyGames()
        {
            var myWebClient = new WebClient();
            string urlString = String.Format("http://localhost:3000/PSN/{0}", ViewBag.UserName);
            var resultBytes = await myWebClient.DownloadDataTaskAsync(urlString);
            string resultStr = System.Text.Encoding.UTF8.GetString(resultBytes);
            if (resultStr.Contains("SUCCESS"))
            {
                return RedirectToAction("Index", "Game");
            }
            else
            {
                return View("Index");
            }

        }

    }
}
