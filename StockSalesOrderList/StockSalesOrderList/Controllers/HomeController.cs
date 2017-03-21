using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    /// <summary>
    /// ホーム＆メニュー用コントローラー
    /// </summary>
    [Authorize]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Menu");

            return View();
        }

        [HttpGet]
        public ActionResult Menu()
        {
            WriteAppLog("MENU", "画面表示");
            return View();
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("disabled")]
        public ActionResult RedirectToMe()
        {
            return RedirectToAction("Menu");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("salesview")]
        public ActionResult redirectToSalesView()
        {
            return RedirectToAction("Index", "SalesView");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("managed")]
        public ActionResult RedirectToManaged()
        {
            return RedirectToAction("Index", "Managed");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("planupload")]
        public ActionResult RedirectToPlanUpload()
        {
            return RedirectToAction("UploadSalesPlan", "SalesView");
            // return RedirectToAction("Plan", "Upload");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("mymaker")]
        public ActionResult RedirectToMyMaker()
        {
            return RedirectToAction("Menu");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("mail")]
        public ActionResult RedirectToMail()
        {
            return RedirectToAction("ChangeMail", "Account");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("change")]
        public ActionResult RedirectToPasswordChange()
        {
            return RedirectToAction("ChangePassword", "Account");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("logview")]
        public ActionResult RedirectToLogView()
        {
            return RedirectToAction("LogView", "Managed");
        }

        [HttpPost]
        [ActionName("Menu"), SubmitCommand("appview")]
        public ActionResult RedirectToApplicationLogView()
        {
            return RedirectToAction("AppView", "Managed");
        }

    }
}
