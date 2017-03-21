using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Services
{
    /// <summary>
    /// 独自エクセル操作サービス(NPOIのカプセル化)
    /// </summary>
    public class CustomExcelService : BaseService
    {
        /// <summary>
        /// １シート版書式指定
        /// </summary>
        protected struct ONE_ROW
        {
            /// <summary>
            /// 全体情報ヘッダ
            /// </summary>
            public const int ROW_HEAD = 0;

            /// <summary>
            /// ループ開始行
            /// </summary>
            public const int ROW_START = 1;

            /// <summary>
            /// １商品の行数
            /// </summary>
            public const int ROWS_ONE_PRODUCT = 17;

            /// <summary>
            /// 月表示の開始列
            /// </summary>
            public const int COL_START_MONTH = 2;

            /// <summary>
            /// 月表示の繰り返し数
            /// </summary>
            public const int COLS_REPEAT_MONTH = 18;

            /// <summary>
            /// 累計表示列
            /// </summary>
            public const int COL_SUMMARY = COL_START_MONTH + COLS_REPEAT_MONTH;

            /// <summary>
            /// 在庫予測の基準からの位置
            /// </summary>
            public const int ROW_STOCK_PLAN = 1;
            /// <summary>
            /// 在庫実績の基準からの位置
            /// </summary>
            public const int ROW_STOCK_ACTUAL = 2;
            /// <summary>
            /// 発注予定の基準からの位置
            /// </summary>
            public const int ROW_ORDER_PLAN = 3;
            /// <summary>
            /// 発注実績の基準からの位置
            /// </summary>
            public const int ROW_ORDER_ACTUAL = 4;
            /// <summary>
            /// 入荷予定の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_PLAN = 5;
            /// <summary>
            /// 入荷実績の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_ACTUAL = 6;
            /// <summary>
            /// 入荷残数の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_REAMING = 7;
            /// <summary>
            /// 入荷残数調整の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_ADJUSTMENT = 8;
            /// <summary>
            /// 販売前年実績の基準からの位置
            /// </summary>
            public const int ROW_SALES_PRE = 9;
            /// <summary>
            /// 販売予算の基準からの位置
            /// </summary>
            public const int ROW_SALES_PLAN = 10;
            /// <summary>
            /// 販売動向の基準からの位置
            /// </summary>
            public const int ROW_SALES_TREND = 11;
            /// <summary>
            /// 販売実績の基準からの位置
            /// </summary>
            public const int ROW_SALES_ACTUAL = 12;
            /// <summary>
            /// 前年比率の基準からの位置
            /// </summary>
            public const int ROW_SALES_PRE_PERCENT = 13;
            /// <summary>
            /// 予実比率の基準からの位置
            /// </summary>
            public const int ROW_SALES_PLAN_PERCENT = 14;
        }

        /// <summary>
        /// タブ区切り版書式設定
        /// </summary>
        protected struct MULTI_CONFIG
        {
            public const int ROW_HEAD = 0;
            public const int ROW_UP_MONTH = 1;
            public const int ROW_DOWN_MONTH = 16;

            /// <summary>
            /// 月表示の開始列
            /// </summary>
            public const int COL_START_MONTH = 2;

            /// <summary>
            /// 月表示の繰り返し数
            /// </summary>
            public const int COLS_REPEAT_MONTH = 18;

            /// <summary>
            /// 累計表示列
            /// </summary>
            public const int COL_SUMMARY = COL_START_MONTH + COLS_REPEAT_MONTH;

            /// <summary>
            /// 在庫予測の基準からの位置
            /// </summary>
            public const int ROW_STOCK_PLAN = 2;
            /// <summary>
            /// 在庫実績の基準からの位置
            /// </summary>
            public const int ROW_STOCK_ACTUAL = 3;
            /// <summary>
            /// 発注予定の基準からの位置
            /// </summary>
            public const int ROW_ORDER_PLAN = 4;
            /// <summary>
            /// 発注実績の基準からの位置
            /// </summary>
            public const int ROW_ORDER_ACTUAL = 5;
            /// <summary>
            /// 入荷予定の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_PLAN = 6;
            /// <summary>
            /// 入荷実績の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_ACTUAL = 7;
            /// <summary>
            /// 入荷残数の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_REAMING = 8;
            /// <summary>
            /// 入荷残数調整の基準からの位置
            /// </summary>
            public const int ROW_INVOICE_ADJUSTMENT = 9;
            /// <summary>
            /// 販売前年実績の基準からの位置
            /// </summary>
            public const int ROW_SALES_PRE = 10;
            /// <summary>
            /// 販売予算の基準からの位置
            /// </summary>
            public const int ROW_SALES_PLAN = 11;
            /// <summary>
            /// 販売動向の基準からの位置
            /// </summary>
            public const int ROW_SALES_TREND = 12;
            /// <summary>
            /// 販売実績の基準からの位置
            /// </summary>
            public const int ROW_SALES_ACTUAL = 13;
            /// <summary>
            /// 前年比率の基準からの位置
            /// </summary>
            public const int ROW_SALES_PRE_PERCENT = 14;
            /// <summary>
            /// 予実比率の基準からの位置
            /// </summary>
            public const int ROW_SALES_PLAN_PERCENT = 15;

            public const int ROW_OFFICE_START = 17;
            public const int ROWS_ONE_OFFICE = 2;
            public const int ROW_OFFICE_PRE = 0;
            public const int ROW_OFFICE_ACTUAL = 1;
        }

        #region EXCEL_CONTENT
        protected const string CONTENT_XLS = "application/vnd.ms-excel";
        protected const string CONTENT_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        #endregion

        #region セル書式補助
        /// <summary>
        /// 書式指定用
        /// </summary>
        protected enum FORMAT
        {
            NORMAL = 0,
            /// <summary>
            /// yyyy/mm/dd
            /// </summary>
            DATE = 1,
            /// <summary>
            /// yyyy年mm月dd日
            /// </summary>
            DATE_LOCAL,
            /// <summary>
            /// 0.1%
            /// </summary>
            PERCENT,
            /// <summary>
            /// #,##0.000
            /// </summary>
            DOUBLE,
            /// <summary>
            /// #,##0
            /// </summary>
            INT,
            /// <summary>
            /// m月
            /// </summary>
            MONTH,
            /// <summary>
            /// yyyy年
            /// </summary>
            YEAR,
            /// <summary>
            /// @
            /// </summary>
            STRING
        }

        /// <summary>
        /// 罫線指定用
        /// </summary>
        protected enum BORDER
        {
            NORMAL = 0,
            /// <summary>
            /// 上直線のみ
            /// </summary>
            TOP_BORDER = 1,
            /// <summary>
            /// 上下左右：直線
            /// </summary>
            BOX,
            /// <summary>
            /// 上：破線 ほか：直線
            /// </summary>
            BOTTOM,
            /// <summary>
            /// 下：破線 ほか：直線
            /// </summary>
            TOP,
            /// <summary>
            /// 上下：破線 ほか：直線
            /// </summary>
            MID,
            /// <summary>
            /// 右：破線 ほか：直線
            /// </summary>
            LEFT,
            /// <summary>
            /// 左右：破線 ほか：直線
            /// </summary>
            CENTER,
            /// <summary>
            /// 左：破線 ほか：直線
            /// </summary>
            RIGHT
        }

        /// <summary>
        /// 水平位置指定用
        /// </summary>
        protected enum HALIGNMENT
        {
            NORMAL = 0,
            /// <summary>
            /// 横方向：中央寄せ
            /// </summary>
            CENTER = 1,
            /// <summary>
            /// 左寄せ
            /// </summary>
            LEFT,
            /// <summary>
            /// 右寄せ
            /// </summary>
            RIGHT,
        }

        /// <summary>
        /// 水平位置指定用
        /// </summary>
        protected enum VALIGNMENT
        {
            NORMAL = 0,
            /// <summary>
            /// 上揃え
            /// </summary>
            TOP,
            /// <summary>
            /// 下揃え
            /// </summary>
            BOTTOM,
            /// <summary>
            /// 縦方向：中央寄せ
            /// </summary>
            MIDDLE
        }

        /// <summary>
        /// カラー指定用
        /// </summary>
        protected enum COLOR
        {
            NORMAL = 0,
            /// <summary>
            /// スカイブルー
            /// </summary>
            SKYBLUE = 1,
            /// <summary>
            /// ライトグリーン
            /// </summary>
            LIGHTGREEN
        }

        /// <summary>
        /// 文字制御指定
        /// </summary>
        protected enum CONTROL
        {
            NORMAL = 0,
            /// <summary>
            /// 縮小表示
            /// </summary>
            SHRINK = 1,
            /// <summary>
            /// 折り返し表示
            /// </summary>
            WRAP
        }

        /// <summary>
        /// フォント指定用
        /// </summary>
        protected enum FONTSTYLE
        {
            NORMAL = 0, // デフォルト(Pゴシック/11)
            PGOTHIC_11_BOLD
        }
        #endregion

        /// <summary>
        /// CELL書式を示すクラス
        /// </summary>
        protected class CellStyle : IEquatable<CellStyle>
        {
            public FORMAT format { get; set; }
            public BORDER border { get; set; }
            public HALIGNMENT alignment { get; set; }
            public VALIGNMENT valignment { get; set; }
            public COLOR color { get; set; }
            public CONTROL control { get; set; }
            public FONTSTYLE font { get; set; }

            public CellStyle()
            {
                format = FORMAT.NORMAL;
                border = BORDER.NORMAL;
                alignment = HALIGNMENT.NORMAL;
                valignment = VALIGNMENT.NORMAL;
                color = COLOR.NORMAL;
                control = CONTROL.NORMAL;
                font = FONTSTYLE.NORMAL;
            }

            public CellStyle(FORMAT format = FORMAT.NORMAL, BORDER border = BORDER.NORMAL,
                HALIGNMENT alignment = HALIGNMENT.NORMAL, VALIGNMENT valignment = VALIGNMENT.NORMAL,
                COLOR color = COLOR.NORMAL, CONTROL control = CONTROL.NORMAL,
                FONTSTYLE font = FONTSTYLE.NORMAL)
            {
                this.format = format;
                this.border = border;
                this.alignment = alignment;
                this.valignment = valignment;
                this.color = color;
                this.control = control;
                this.font = font;
            }

            public override int GetHashCode()
            {
                return format.GetHashCode() ^ border.GetHashCode()
                    ^ alignment.GetHashCode() ^ valignment.GetHashCode()
                    ^ color.GetHashCode() ^ control.GetHashCode() ^ font.GetHashCode();
            }

            bool IEquatable<CellStyle>.Equals(CellStyle other)
            {
                if (other == null || this.GetType() != other.GetType())
                {
                    return false;
                }
                return format == other.format && border == other.border
                    && alignment == other.alignment && valignment == other.valignment
                    && color == other.color && control == other.control && font == other.font;
            }
        }

        #region 変数宣言
        /// <summary>
        /// ブック管理用
        /// </summary>
        private IWorkbook book = null;
        /// <summary>
        /// デフォルト処理対象シート
        /// </summary>
        private ISheet targetSheet = null;
        /// <summary>
        /// 日付書式作業用
        /// </summary>
        private IDataFormat format = null;
        /// <summary>
        /// CellStyleを保持
        /// </summary>
        private Dictionary<CellStyle, ICellStyle> styles;
        /// <summary>
        /// Fontを保持
        /// </summary>
        private Dictionary<FONTSTYLE, IFont> fonts;
        /// <summary>
        /// 新規作成時のEXCELで旧式を使用
        /// </summary>
        private bool isOldMode = false;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomExcelService() : base()
        {
            styles = new Dictionary<CellStyle, ICellStyle>();
            fonts = new Dictionary<FONTSTYLE, IFont>();
        }

        #region ブックデータ操作
        /// <summary>
        /// 新規ブックを作成
        /// </summary>
        /// <returns>作成の成否</returns>
        protected bool CreateBook()
        {
            try
            {
                if (book != null)
                    book.Close();

                if (isOldMode)
                    book = new HSSFWorkbook();
                else
                    book = new XSSFWorkbook();
                styles.Clear();
                fonts.Clear();
                format = book.CreateDataFormat();
                return true;
            }
            catch (Exception)
            {
                book = null;
                return false;
            }
        }

        /// <summary>
        /// 既存ブックを開く
        /// </summary>
        /// <param name="dataStream">対象データのストリーム</param>
        /// <returns>処理の成否</returns>
        protected bool OpenBook(Stream dataStream)
        {
            try
            {
                if (book != null)
                    book.Close();

                book = WorkbookFactory.Create(dataStream);
                styles.Clear();
                fonts.Clear();
                format = book.CreateDataFormat();
                return true;
            }
            catch (Exception)
            {
                book = null;
                return false;
            }
        }

        /// <summary>
        /// ブックを閉じる
        /// </summary>
        protected void CloseBook()
        {
            if (book != null)
                book.Close();
            book = null;
            styles.Clear();
            fonts.Clear();
            format = null;
        }

        /// <summary>
        /// 処理対象ブック
        /// </summary>
        protected IWorkbook WorkBook
        {
            get
            {
                return this.book;
            }
        }

        #endregion

        #region シート操作
        /// <summary>
        /// シート情報を取得/処理対象シートを設定
        /// </summary>
        /// <param name="name">シート名</param>
        /// <returns>シート</returns>
        protected ISheet GetSheetByName(string name)
        {
            targetSheet = book.GetSheet(name) ?? book.CreateSheet(name);
            return targetSheet;
        }

        /// <summary>
        /// シート情報を取得
        /// 存在する場合は対象シートを更新、存在しない場合は更新しない
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>シート/存在しない場合はNULL</returns>
        protected ISheet GetSheetByIndex(int index)
        {
            try
            {
                ISheet tmp = book.GetSheetAt(index);
                if (tmp != null)
                    targetSheet = tmp;
                return tmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 処理対象シート
        /// </summary>
        protected ISheet TargetSheet
        {
            get
            {
                return this.targetSheet;
            }
            set
            {
                this.targetSheet = value;
            }
        }
        #endregion

        #region CELLスタイル管理
        /// <summary>
        /// スタイル情報を取得
        /// </summary>
        /// <param name="mode">スタイル指定クラス</param>
        /// <returns>スタイル情報</returns>
        private ICellStyle GetStyle(CellStyle mode)
        {
            // 登録済みのスタイルは流用
            if (styles.ContainsKey(mode))
                return styles[mode];

            // 未登録の場合は作成
            var style = book.CreateCellStyle();

            // 書式処理
            switch (mode.format)
            {
                case FORMAT.DATE:
                    style.DataFormat = format.GetFormat("yyyy/mm/dd");
                    break;
                case FORMAT.DATE_LOCAL:
                    // style.DataFormat = format.GetFormat("yyyy\"年\"m\"月\"d\"日\";@");
                    style.DataFormat = format.GetFormat("[$-F800]dddd, mmmm dd, yyyy");
                    break;
                case FORMAT.PERCENT:
                    style.DataFormat = format.GetFormat("0.0%");
                    break;
                case FORMAT.DOUBLE:
                    style.DataFormat = format.GetFormat("#,##0.000_ ;[Red]-#,##0.000 ");
                    break;
                case FORMAT.INT:
                    style.DataFormat = format.GetFormat("#,##0_ ;[Red]-#,##0 ");
                    break;
                case FORMAT.MONTH:
                    style.DataFormat = format.GetFormat("[$-409]m\"月\";@");
                    break;
                case FORMAT.YEAR:
                    style.DataFormat = format.GetFormat("[$-409]yyyy\"年\";@");
                    break;
                case FORMAT.STRING:
                    style.DataFormat = format.GetFormat("@");
                    break;
            }

            // 罫線
            switch (mode.border)
            {
                case BORDER.TOP_BORDER:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.None;
                    style.BorderLeft = BorderStyle.None;
                    style.BorderRight = BorderStyle.None;
                    break;
                case BORDER.BOX:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    break;
                case BORDER.BOTTOM:
                    style.BorderTop = BorderStyle.Dotted;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    break;
                case BORDER.TOP:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Dotted;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    break;
                case BORDER.MID:
                    style.BorderTop = BorderStyle.Dotted;
                    style.BorderBottom = BorderStyle.Dotted;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    break;
                case BORDER.LEFT:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Dotted;
                    break;
                case BORDER.CENTER:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Dotted;
                    style.BorderRight = BorderStyle.Dotted;
                    break;
                case BORDER.RIGHT:
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Dotted;
                    style.BorderRight = BorderStyle.Thin;
                    break;
                default:
                    style.BorderTop = BorderStyle.None;
                    style.BorderBottom = BorderStyle.None;
                    style.BorderLeft = BorderStyle.None;
                    style.BorderRight = BorderStyle.None;
                    break;
            }

            // 水平位置
            switch (mode.alignment)
            {
                case HALIGNMENT.CENTER:
                    style.Alignment = HorizontalAlignment.Center;
                    break;
                case HALIGNMENT.LEFT:
                    style.Alignment = HorizontalAlignment.Left;
                    break;
                case HALIGNMENT.RIGHT:
                    style.Alignment = HorizontalAlignment.Right;
                    break;
                default:
                    style.Alignment = HorizontalAlignment.General;
                    break;
            }

            // 垂直位置
            switch (mode.valignment)
            {
                case VALIGNMENT.TOP:
                    style.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case VALIGNMENT.BOTTOM:
                    style.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case VALIGNMENT.MIDDLE:
                    style.VerticalAlignment = VerticalAlignment.Center;
                    break;
                default:
                    style.VerticalAlignment = VerticalAlignment.Center;
                    break;
            }

            // 色
            switch (mode.color)
            {
                case COLOR.SKYBLUE:
                    style.FillForegroundColor = IndexedColors.SkyBlue.Index;
                    style.FillPattern = FillPattern.SolidForeground;
                    break;
                case COLOR.LIGHTGREEN:
                    style.FillForegroundColor = IndexedColors.LightGreen.Index;
                    style.FillPattern = FillPattern.SolidForeground;
                    break;
            }

            // 文字制御
            switch (mode.control)
            {
                case CONTROL.SHRINK:
                    style.ShrinkToFit = true;
                    style.WrapText = false;
                    break;
                case CONTROL.WRAP:
                    style.ShrinkToFit = false;
                    style.WrapText = true;
                    break;
                default:
                    style.ShrinkToFit = false;
                    style.WrapText = false;
                    break;
            }

            // フォント
            switch (mode.font)
            {
                case FONTSTYLE.NORMAL:
                    break;
                default:
                    style.SetFont(GetFont(FONTSTYLE.PGOTHIC_11_BOLD));
                    break;
            }

            styles.Add(mode, style);
            return style;
        }
        #endregion

        #region フォント管理
        private IFont GetFont(FONTSTYLE mode)
        {
            // 登録済みのスタイルは流用
            if (fonts.ContainsKey(mode))
                return fonts[mode];

            // 未登録の場合は作成
            var font = book.CreateFont();
            if (mode == FONTSTYLE.PGOTHIC_11_BOLD)
            {
                font.FontName = "ＭＳ Ｐゴシック";
                font.FontHeightInPoints = 11;
                font.IsBold = true;
            }
            else if (mode == FONTSTYLE.NORMAL)
            {
                font.FontName = "ＭＳ Ｐゴシック";
                font.FontHeightInPoints = 11;
            }
            fonts.Add(mode, font);
            return font;
        }
        #endregion

        #region CELL書き込み
        /// <summary>
        /// 指定セルにdecimal値を書き込む
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(int columnIndex, int rowIndex, decimal value, CellStyle style = null)
        {
            WriteCell(targetSheet, columnIndex, rowIndex, value, style);
        }
        /// <summary>
        /// 指定セルに文字列値を書き込む
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(int columnIndex, int rowIndex, string value, CellStyle style = null)
        {
            WriteCell(targetSheet, columnIndex, rowIndex, value, style);
        }
        /// <summary>
        /// 指定セルに日付値を書き込む
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(int columnIndex, int rowIndex, DateTime value, CellStyle style = null)
        {
            WriteCell(targetSheet, columnIndex, rowIndex, value, style);
        }
        /// <summary>
        /// 指定セルに計算式を書き込む
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="formula">計算式</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteFormula(int columnIndex, int rowIndex, string formula, CellStyle style = null)
        {
            WriteFormula(targetSheet, columnIndex, rowIndex, formula, style);
        }
        /// <summary>
        /// 指定セルにdecimal値を書き込む
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(ISheet sheet, int columnIndex, int rowIndex, decimal value, CellStyle style = null)
        {
            IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            ICell cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.SetCellValue((double)value);
            if (style != null)
                WriteStyle(sheet, columnIndex, rowIndex, style);
        }
        /// <summary>
        /// 指定セルに文字列値を書き込む
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(ISheet sheet, int columnIndex, int rowIndex, string value, CellStyle style = null)
        {
            IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            ICell cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.SetCellValue(value);
            if (style != null)
                WriteStyle(sheet, columnIndex, rowIndex, style);
        }
        /// <summary>
        /// 指定セルに日付値を書き込む
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">値</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteCell(ISheet sheet, int columnIndex, int rowIndex, DateTime value, CellStyle style = null)
        {
            IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            ICell cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.SetCellValue(value);
            if (style != null)
                WriteStyle(sheet, columnIndex, rowIndex, style);
        }
        /// <summary>
        /// 指定セルに計算式を書き込む
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="formula">計算式</param>
        /// <param name="style">(省略可能)スタイル</param>
        protected void WriteFormula(ISheet sheet, int columnIndex, int rowIndex, string formula, CellStyle style = null)
        {
            IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            ICell cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.SetCellFormula(formula);
            if (style != null)
                WriteStyle(sheet, columnIndex, rowIndex, style);
        }
        #endregion

        #region CELLスタイル設定
        /// <summary>
        /// 指定セルのCELLSTYLEを取得
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>ICellStyle</returns>
        protected ICellStyle GetCellStyle(int columnIndex, int rowIndex)
        {
            return GetCellStyle(targetSheet, columnIndex, rowIndex);
        }
        /// <summary>
        /// 指定セルのCELLSTYLEを取得
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>ICellStyle</returns>
        protected ICellStyle GetCellStyle(ISheet sheet, int columnIndex, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            return cell.CellStyle;
        }
        /// <summary>
        /// 指定セルにstyleを適用
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="style">ICellStyle</param>
        protected void WriteStyle(int columnIndex, int rowIndex, ICellStyle style)
        {
            WriteStyle(targetSheet, columnIndex, rowIndex, style);
        }
        /// <summary>
        /// 指定セルにstyleを適用
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="style">ICellStyle</param>
        protected void WriteStyle(ISheet sheet, int columnIndex, int rowIndex, ICellStyle style)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.CellStyle = style;
        }
        /// <summary>
        /// 指定セルにCellStyleクラスの内容のスタイルを適用
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="mode">スタイルクラス</param>
        protected void WriteStyle(int columnIndex, int rowIndex, CellStyle mode)
        {
            WriteStyle(targetSheet, columnIndex, rowIndex, mode);
        }
        /// <summary>
        /// 指定セルにCellStyleクラスの内容のスタイルを適用
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="mode">スタイルクラス</param>
        protected void WriteStyle(ISheet sheet, int columnIndex, int rowIndex, CellStyle mode)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.CellStyle = GetStyle(mode);
        }
        #endregion

        #region CELL読み取り
        /// <summary>
        /// セルのタイプを取得
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>CellType</returns>
        protected CellType? GetCellType(ISheet sheet, int columnIndex, int rowIndex)
        {
            IRow row = sheet?.GetRow(rowIndex);
            ICell cell = row?.GetCell(columnIndex);
            return cell?.CellType;
        }
        /// <summary>
        /// セルのタイプが日付型か判定
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>判定結果</returns>
        protected bool CheckCellTypeDate(ISheet sheet, int columnIndex, int rowIndex)
        {
            IRow row = sheet?.GetRow(rowIndex);
            ICell cell = row?.GetCell(columnIndex);
            if (cell?.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                return true;
            return false;
        }
        /// <summary>
        /// 文字列として値を取得
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">文字列型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(int columnIndex, int rowIndex, out string value)
        {
            return TryReadCell(targetSheet, columnIndex, rowIndex, out value);
        }
        /// <summary>
        /// decimalとして値を取得
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">decimal型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(int columnIndex, int rowIndex, out decimal value)
        {
            return TryReadCell(targetSheet, columnIndex, rowIndex, out value);
        }
        /// <summary>
        /// 日時型として値を取得
        /// </summary>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">日時型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(int columnIndex, int rowIndex, out DateTime value)
        {
            return TryReadCell(targetSheet, columnIndex, rowIndex, out value);
        }
        /// <summary>
        /// 文字列として値を取得
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">文字列型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(ISheet sheet, int columnIndex, int rowIndex, out string value)
        {
            try
            {
                IRow row = sheet?.GetRow(rowIndex);
                ICell cell = row?.GetCell(columnIndex);
                value = cell?.StringCellValue;

                if (value == null)
                    return false;
                return true;
            }
            catch (Exception)
            {
                value = string.Empty;
                return false;
            }
        }
        /// <summary>
        /// decimalとして値を取得
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">decimal型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(ISheet sheet, int columnIndex, int rowIndex, out decimal value)
        {
            try
            {
                IRow row = sheet?.GetRow(rowIndex);
                ICell cell = row?.GetCell(columnIndex);
                double? result = cell?.NumericCellValue;

                value = 0;
                if (!result.HasValue)
                    return false;

                value = (decimal)result;
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        /// <summary>
        /// 日時型として値を取得
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <param name="value">日時型変数</param>
        /// <returns>取得の成否</returns>
        protected bool TryReadCell(ISheet sheet, int columnIndex, int rowIndex, out DateTime value)
        {
            try
            {
                IRow row = sheet?.GetRow(rowIndex);
                ICell cell = row?.GetCell(columnIndex);
                DateTime? result = cell?.DateCellValue;

                if (!result.HasValue)
                {
                    value = new DateTime();
                    return false;
                }
                value = (DateTime)result;
                return true;
            }
            catch (Exception)
            {
                value = new DateTime();
                return false;
            }
        }
        /// <summary>
        /// セルの内容を取得する
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="columnIndex">列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>取得文字列</returns>
        protected string ReadCellString(ISheet sheet, int columnIndex, int rowIndex)
        {
            IRow row = sheet?.GetRow(rowIndex);
            ICell cell = row?.GetCell(columnIndex);
            return cell?.StringCellValue;
        }
        #endregion

        #region CELL結合
        /// <summary>
        /// セルの結合
        /// </summary>
        /// <param name="firstCol">左列</param>
        /// <param name="firstRow">上行</param>
        /// <param name="lastCol">右列</param>
        /// <param name="lastRow">下行</param>
        /// <returns>処理の成否</returns>
        protected bool MergedCell(int firstCol, int firstRow, int lastCol, int lastRow)
        {
            return MergedCell(targetSheet, firstCol, firstRow, lastCol, lastRow);
        }
        /// <summary>
        /// セルの結合
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="firstCol">左列</param>
        /// <param name="firstRow">上行</param>
        /// <param name="lastCol">右列</param>
        /// <param name="lastRow">下行</param>
        /// <returns>処理の成否</returns>
        protected bool MergedCell(ISheet sheet, int firstCol, int firstRow, int lastCol, int lastRow)
        {
            if (firstCol < 0 || firstRow < 0)
                return false;
            if (firstCol > lastCol || firstRow > lastRow)
                return false;

            sheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
            ICellStyle _style = GetCellStyle(sheet, firstCol, firstRow);
            for (int colIndex = firstCol; colIndex <= lastCol; colIndex++)
            {
                for (int rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
                {
                    WriteStyle(sheet, colIndex, rowIndex, _style);
                }
            }
            return true;
        }
        /// <summary>
        /// セルの結合（サイズ指定）
        /// </summary>
        /// <param name="firstCol">基準列</param>
        /// <param name="firstRow">基準行</param>
        /// <param name="colWidth">列サイズ(１～)</param>
        /// <param name="rowHeight">行サイズ(１～)</param>
        /// <returns>処理の成否</returns>
        protected bool MergedCellRange(int firstCol, int firstRow, int colWidth, int rowHeight)
        {
            if (colWidth < 0 || rowHeight < 0)
                return false;
            return MergedCellRange(targetSheet, firstCol, firstRow, colWidth, rowHeight);
        }
        /// <summary>
        /// セルの結合（サイズ指定）
        /// </summary>
        /// <param name="sheet">対象シート</param>
        /// <param name="firstCol">基準列</param>
        /// <param name="firstRow">基準行</param>
        /// <param name="colWidth">列サイズ(１～)</param>
        /// <param name="rowHeight">行サイズ(１～)</param>
        /// <returns>処理の成否</returns>
        protected bool MergedCellRange(ISheet sheet, int firstCol, int firstRow, int colWidth, int rowHeight)
        {
            if (colWidth <= 0 || rowHeight <= 0)
                return false;
            return MergedCell(sheet, firstCol, firstRow, firstCol + colWidth, firstRow + rowHeight);
        }
        #endregion
    }
}
