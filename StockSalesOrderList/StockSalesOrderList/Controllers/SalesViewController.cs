using Microsoft.Web.Mvc;
using StockSalesOrderList.Helpers;
using StockSalesOrderList.Models;
using StockSalesOrderList.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    [Authorize]
    public class SalesViewController : BaseController
    {
        private const string CONTENT_XLS = "application/vnd.ms-excel";
        private const string CONTENT_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MakerView()
        {
            WriteAppLog("対象メーカー選択", "画面表示");
            return PartialView("_MakerView");
        }

        public ActionResult GroupView()
        {
            WriteAppLog("対象グループ選択", "画面表示");
            return PartialView("_GroupView");
        }

        public ActionResult SalesView()
        {
            WriteAppLog("在庫販売情報一覧", "画面表示");
            return PartialView("_SalesView");
        }

        public ActionResult ProcustDetail()
        {
            WriteAppLog("商品明細表示", "画面表示");
            return PartialView("_ProductView");
        }

        [HttpGet]
        public ActionResult UploadSalesPlan()
        {
            WriteAppLog("予算アップロード", "画面表示");
            return View();
            // 念のためアップロードされたファイルを保管
            /*
            string upfile = Server.MapPath("~/App_Data/Uploaded/")
                + Path.GetFileNameWithoutExtension(model.UploadFile.FileName)
                + "_" + GetUserId().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                + Path.GetExtension(model.UploadFile.FileName);
            if (!System.IO.File.Exists(upfile))
                model.UploadFile.SaveAs(upfile);
            */
            // return Content("完了");
        }

        [HttpPost]
        public ActionResult UploadSalesPlan(UploadSalesPlanViewModel model)
        {
            if(model==null || model.file==null)
                return HttpStatucCodeWithContent("処理対象ファイルがありません。", HttpStatusCode.BadRequest);
            if (model.file.ContentLength == 0)
                return HttpStatucCodeWithContent("中身がありません", HttpStatusCode.BadRequest);



            /*
            // テスト検証のために保管
            string upfile = Server.MapPath("~/App_Data/Uploaded/")
            + Path.GetFileNameWithoutExtension(model.file.FileName)
            + "_" + GetUserId().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
            + Path.GetExtension(model.file.FileName);
            if (!System.IO.File.Exists(upfile))
                model.file.SaveAs(upfile);

            WriteAppLog("予算アップロード", "アップロード：" + upfile);

            UploadService service = new UploadService();
            string message = string.Empty;

            if (!service.ReadXlsxToSalesView(model.file.InputStream, out message))
            {
                WriteAppLog("予算アップロード", "アップロード失敗：" + message);
                return HttpStatucCodeWithContent(message, HttpStatusCode.BadRequest);
            }
            */

            WriteAppLog("予算アップロード", "アップロードファイル：" + model.file.FileName);

            ImportService service = new ImportService();
            string message = string.Empty;

            if (!service.ReadXlsxToSalesView(model.file.InputStream, out message))
            {
                WriteAppLog("予算アップロード", "アップロード失敗：" + message);
                return HttpStatucCodeWithContent(message, HttpStatusCode.BadRequest);
            }

            WriteAppLog("予算アップロード", "アップロード成功");
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// 販売在庫データのダウンロード
        /// </summary>
        /// <param name="year"></param>
        /// <param name="groupid"></param>
        /// <param name="makerid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download(int? year, int? groupid, int? makerid)
        {
            if (!year.HasValue)
                return Content("パラメータが不足");
            if (!groupid.HasValue && !makerid.HasValue)
                return Content("パラメータが不足");

            ExportService service = new ExportService();
            string dlname = "Download";
            dlname += year.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmssff") + ".xlsx";
            byte[] result = service.CreateXlsxOneSheetBySalesView((int)year, groupid, makerid);
            if (result == null || result.Length == 0)
            {
                WriteAppLog("データダウンロード", "ダウンロード失敗");
                return Content("データが存在しない？");
            }
            WriteAppLog("データダウンロード", "ダウンロード成功：" + dlname);
            return File(result, CONTENT_XLSX, dlname);
        }

        /// <summary>
        /// 販売在庫データのダウンロード
        /// </summary>
        /// <param name="year"></param>
        /// <param name="groupid"></param>
        /// <param name="makerid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownloadTab(int? year, int? groupid, int? makerid)
        {
            if (!year.HasValue)
                return Content("パラメータが不足");
            if (!groupid.HasValue && !makerid.HasValue)
                return Content("パラメータが不足");

            ExportService service = new ExportService();
            string dlname = "DownloadTab";
            dlname += year.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmssff") + ".xlsx";
            byte[] result = service.CreateXlsxBySalesViewTab((int)year, groupid, makerid);
            if (result == null || result.Length == 0)
            {
                WriteAppLog("データダウンロード(タブ型)", "ダウンロード失敗");
                return Content("データが存在しない？");
            }
            WriteAppLog("データダウンロード(タブ型)", "ダウンロード成功：" + dlname);
            return File(result, CONTENT_XLSX, dlname);
        }
    }
}
