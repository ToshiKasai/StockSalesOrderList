using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    [Authorize]
    public class ManagedController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuView()
        {
            WriteAppLog("管理メニュー", "画面表示");
            return PartialView("_Menu");
        }

        #region ユーザー画面テンプレート
        public ActionResult UserList()
        {
            WriteAppLog("ユーザー管理", "画面表示");
            return PartialView("_UserList");
        }

        public ActionResult UserEdit()
        {
            WriteAppLog("ユーザー編集", "画面表示");
            return PartialView("_UserEdit");
        }

        public ActionResult UserNew()
        {
            WriteAppLog("ユーザー新規登録", "画面表示");
            return PartialView("_UserNew");
        }

        public ActionResult UserRole()
        {
            WriteAppLog("ユーザーロール編集", "画面表示");
            return PartialView("_UserRole");
        }

        public ActionResult UserMaker()
        {
            WriteAppLog("ユーザーメーカー編集", "画面表示");
            return PartialView("_UserMaker");
        }
        #endregion

        #region メーカー画面テンプレート
        public ActionResult MakerList()
        {
            WriteAppLog("メーカー管理", "画面表示");
            return PartialView("_MakerList");
        }
        #endregion

        #region グループ画面テンプレート
        public ActionResult GroupList()
        {
            WriteAppLog("グループ管理", "画面表示");
            return PartialView("_GroupList");
        }
        public ActionResult GroupEdit()
        {
            WriteAppLog("グループ編集", "画面表示");
            return PartialView("_GroupEdit");
        }
        public ActionResult GroupProduct()
        {
            WriteAppLog("グループ商品編集", "画面表示");
            return PartialView("_GroupProduct");
        }
        #endregion

        #region 商品画面テンプレート
        public ActionResult ProductList()
        {
            WriteAppLog("商品管理", "画面表示");
            return PartialView("_ProductList");
        }
        public ActionResult ProductEdit()
        {
            WriteAppLog("商品編集", "画面表示");
            return PartialView("_ProductEdit");
        }
        #endregion

        #region ログ関連
        [HttpGet]
        public ActionResult LogView()
        {
            LogViewModel model = new LogViewModel();
            model.dateFrom = DateTime.Now.Date.AddDays(-7);
            model.dateTo = DateTime.Now.Date.AddDays(1);
            model.SigninCode = null;
            model.logs = dbContext.SignInLogModels.Where(sil => sil.ProcessingDate >= model.dateFrom)
                .Where(sil => sil.ProcessingDate <= model.dateTo).OrderByDescending(sil => sil.ProcessingDate).ToList();
            model.dateTo = model.dateTo.Value.AddDays(-1);
            return View(model);
        }

        [HttpPost]
        public ActionResult LogView(LogViewModel model)
        {
            IQueryable<SignInLogModel> query = dbContext.SignInLogModels;
            if (model.dateFrom.HasValue)
                query = query.Where(sil => sil.ProcessingDate >= model.dateFrom.Value);
            if (model.dateTo.HasValue)
            {
                DateTime toExtend = model.dateTo.Value.AddDays(1);
                query = query.Where(sil => sil.ProcessingDate <= toExtend);
            }
            if (!string.IsNullOrEmpty( model.SigninCode))
                query = query.Where(sil => sil.UserCode == model.SigninCode);

            model.logs = query.OrderByDescending(sil => sil.ProcessingDate).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult AppView()
        {
            AppLogViewModel model = new AppLogViewModel();
            model.dateFrom = DateTime.Now.Date.AddDays(-7);
            model.dateTo = DateTime.Now.Date.AddDays(1);
            model.SigninCode = null;
            model.logs = dbContext.ApplicationLogModel.Where(al=>al.ProcessingDate>=model.dateFrom)
                .Where(al=>al.ProcessingDate<model.dateTo)
                .OrderByDescending(al => al.ProcessingDate).ThenByDescending(al => al.Id).ToList();
            model.dateTo = model.dateTo.Value.AddDays(-1);
            return View(model);
        }

        [HttpPost]
        public ActionResult AppView(AppLogViewModel model)
        {
            IQueryable<ApplicationLogModel> query = dbContext.ApplicationLogModel;
            if (model.dateFrom.HasValue)
                query = query.Where(al => al.ProcessingDate >= model.dateFrom.Value);
            if (model.dateTo.HasValue)
            {
                DateTime toExtend = model.dateTo.Value.AddDays(1);
                query = query.Where(al => al.ProcessingDate <= toExtend);
            }
            if (!string.IsNullOrEmpty(model.SigninCode))
                query = query.Where(al => al.UserModel.UserName == model.SigninCode);

            model.logs = query.OrderByDescending(sil => sil.ProcessingDate).ThenByDescending(al => al.Id).ToList();
            return View(model);
        }
        #endregion
    }
}
