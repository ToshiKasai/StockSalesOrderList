using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Services
{
    /// <summary>
    /// 祝日一覧作成用サービス
    /// </summary>
    public class HolidayService
    {
        /// <summary>
        /// 休日データ管理
        /// </summary>
        public class ItemHolidayModel : BaseModel
        {
            public ItemHolidayModel() { }
            public ItemHolidayModel(int type,string title)
            {
                this.type = type;
                this.title = title;
            }
            /// <summary>
            /// 休日タイプ
            ///  0:祝日法に基づく休日
            /// </summary>
            public int type { get; set; }
            /// <summary>
            /// 休日名
            /// </summary>
            public string title { get; set; }
        }

        /// <summary>
        /// 祝日データ定義データ管理
        /// </summary>
        protected class ConfigHoliday : BaseModel
        {
            public ConfigHoliday() { }
            public ConfigHoliday(int fromYear, int toYear, int month, object days, string title)
            {
                this.fromYear = fromYear;
                this.toYear = toYear;
                this.month = month;
                this.days = days;
                this.title = title;
            }
            public ConfigHoliday(int fromYear, int toYear, int month, object days, string title, int type)
            {
                this.fromYear = fromYear;
                this.toYear = toYear;
                this.month = month;
                this.days = days;
                this.title = title;
                this.type = type;
            }
            public int fromYear { get; set; }
            public int toYear { get; set; }
            public int month { get; set; }
            public object days { get; set; }
            public string title { get; set; }
            public int type { get; set; }
        }

        /// <summary>
        /// 休日一覧を格納（返却用）
        /// </summary>
        private Dictionary<string, ItemHolidayModel> holidayList;

        /// <summary>
        /// 振替休日施行開始
        /// </summary>
        private DateTime dateFurikae = DateTime.Parse("1973/4/12");
        /// <summary>
        /// 振替休日の名称
        /// </summary>
        private string titleFurikae = "振替休日";
        /// <summary>
        /// 国民の休日施行開始
        /// </summary>
        private DateTime dateKokumin = DateTime.Parse("1985/12/27");
        /// <summary>
        /// 国民の休日の名称
        /// </summary>
        private string titleKokumin = "国民の休日";

        /// <summary>
        /// 春分の日計算を示す値
        /// </summary>
        private const int Syunbun = -1;
        /// <summary>
        /// 秋分の日計算を示す値
        /// </summary>
        private const int Shuubun = -2;

        /// <summary>
        /// 祝日データ定義一覧
        /// </summary>
        private List<ConfigHoliday> holidays;

        public HolidayService()
        {
            holidayList = new Dictionary<string, ItemHolidayModel>();
            holidays = new List<ConfigHoliday>();
            // 祝日データ設定
            holidays.Add(new ConfigHoliday(1874, 1484, 1, 1, "四方節"));
            holidays.Add(new ConfigHoliday(1949, 9999, 1, 1, "元日"));
            holidays.Add(new ConfigHoliday(1949, 1948, 1, 3, "元始祭"));
            holidays.Add(new ConfigHoliday(1874, 1948, 1, 5, "新年宴会"));
            holidays.Add(new ConfigHoliday(1874, 1999, 1, 15, "成人の日"));
            holidays.Add(new ConfigHoliday(2000, 9999, 1, new int[] { 2, 1 }, "成人の日"));
            holidays.Add(new ConfigHoliday(1874, 1912, 1, 30, "孝明天皇祭"));
            holidays.Add(new ConfigHoliday(1874, 1948, 2, 1, "紀元節"));
            holidays.Add(new ConfigHoliday(1967, 9999, 2, 11, "建国記念の日"));
            holidays.Add(new ConfigHoliday(1989, 1989, 2, 24, "昭和天皇の大喪の礼"));
            holidays.Add(new ConfigHoliday(1879, 1948, 3, Syunbun, "春季皇霊祭"));
            holidays.Add(new ConfigHoliday(1949, 2199, 3, Syunbun, "春分の日"));
            holidays.Add(new ConfigHoliday(1874, 1948, 4, 3, "神武天皇祭"));
            holidays.Add(new ConfigHoliday(1959, 1959, 4, 10, "皇太子・明仁親王の結婚の儀"));
            holidays.Add(new ConfigHoliday(1927, 1948, 4, 29, "天長節"));
            holidays.Add(new ConfigHoliday(1949, 1988, 4, 29, "天皇誕生日"));
            holidays.Add(new ConfigHoliday(1989, 2006, 4, 29, "みどりの日"));
            holidays.Add(new ConfigHoliday(2007, 9999, 4, 29, "昭和の日"));
            holidays.Add(new ConfigHoliday(1949, 9999, 5, 3, "憲法記念日"));
            holidays.Add(new ConfigHoliday(2007, 9999, 5, 4, "みどりの日"));
            holidays.Add(new ConfigHoliday(1949, 9999, 5, 5, "こどもの日"));
            holidays.Add(new ConfigHoliday(1993, 1993, 6, 9, "皇太子・徳仁親王の結婚の儀"));
            holidays.Add(new ConfigHoliday(1996, 2002, 7, 20, "海の日"));
            holidays.Add(new ConfigHoliday(2003, 9999, 7, new int[] { 3, 1 }, "海の日"));
            holidays.Add(new ConfigHoliday(1913, 1926, 7, 30, "明治天皇祭"));
            holidays.Add(new ConfigHoliday(2016, 9999, 8, 11, "山の日"));
            holidays.Add(new ConfigHoliday(1913, 1926, 8, 31, "天長節"));
            holidays.Add(new ConfigHoliday(1966, 2002, 9, 15, "敬老の日"));
            holidays.Add(new ConfigHoliday(2003, 9999, 9, new int[] { 3, 1 }, "敬老の日"));
            holidays.Add(new ConfigHoliday(1874, 1878, 9, 17, "神嘗祭"));
            holidays.Add(new ConfigHoliday(1878, 1947, 9, Shuubun, "秋季皇霊祭"));
            holidays.Add(new ConfigHoliday(1948, 2199, 9, Shuubun, "秋分の日"));
            holidays.Add(new ConfigHoliday(1966, 1999, 10, 10, "体育の日"));
            holidays.Add(new ConfigHoliday(2000, 9999, 10, new int[] { 2, 1 }, "体育の日"));
            holidays.Add(new ConfigHoliday(1873, 1879, 10, 17, "神嘗祭"));
            holidays.Add(new ConfigHoliday(1913, 1926, 10, 31, "天長節祝日"));
            holidays.Add(new ConfigHoliday(1873, 1911, 11, 3, "天長節"));
            holidays.Add(new ConfigHoliday(1927, 1947, 11, 3, "明治節"));
            holidays.Add(new ConfigHoliday(1948, 9999, 11, 3, "文化の日"));
            holidays.Add(new ConfigHoliday(1990, 1990, 11, 12, "即位の礼正殿の儀"));
            holidays.Add(new ConfigHoliday(1873, 1947, 11, 23, "新嘗祭"));
            holidays.Add(new ConfigHoliday(1948, 9999, 11, 23, "勤労感謝の日"));
            holidays.Add(new ConfigHoliday(1915, 1915, 11, 10, "即位の礼"));
            holidays.Add(new ConfigHoliday(1915, 1915, 11, 14, "大嘗祭"));
            holidays.Add(new ConfigHoliday(1915, 1915, 11, 16, "大饗第1日"));
            holidays.Add(new ConfigHoliday(1928, 1928, 11, 10, "即位の礼"));
            holidays.Add(new ConfigHoliday(1928, 1928, 11, 14, "大嘗祭"));
            holidays.Add(new ConfigHoliday(1928, 1928, 11, 16, "大饗第1日"));
            holidays.Add(new ConfigHoliday(1989, 9999, 12, 23, "天皇誕生日"));
            holidays.Add(new ConfigHoliday(1927, 1947, 12, 25, "大正天皇祭"));

            // カスタム休日の定義可能
            holidays.Add(new ConfigHoliday(2017, 2017, 1, 3, "会社休日", 1));
            holidays.Add(new ConfigHoliday(2017, 2017, 1, 4, "会社休日", 1));
        }

        private void setObject(DateTime date, ItemHolidayModel data)
        {
            this.holidayList.Add(date.ToString("yyyy/MM/dd"), data);
        }

        private bool inObject(DateTime date)
        {
            ItemHolidayModel data;
            return this.holidayList.TryGetValue(date.ToString("yyyy/MM/dd"), out data);
        }

        /// <summary>
        /// 指定週の指定曜日の日付を返す
        /// </summary>
        /// <param name="date">基準日付</param>
        /// <param name="count">週</param>
        /// <param name="day">曜日</param>
        /// <returns></returns>
        private DateTime getDayCountInMonth(DateTime date, int count, int day)
        {
            int days = day - (int)date.DayOfWeek + 1;
            days += days < 1 ? count * 7 : (count - 1) * 7;
            return DateTime.Parse(date.Year + "/" + date.Month + "/" + days);
        }

        /// <summary>
        /// 春分の日相当の日付を返す
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private DateTime getSyunbun(DateTime date, int year)
        {
            int surplus = year % 4;
            int day = 20;
            if (1800 <= year && year <= 1827)
            {
                day = 21;
            }
            else if (1828 <= year && year <= 1859)
            {
                day = surplus < 1 ? 20 : 21;
            }
            else if (1860 <= year && year <= 1891)
            {
                day = surplus < 2 ? 20 : 21;
            }
            else if (1892 <= year && year <= 1899)
            {
                day = surplus < 3 ? 20 : 21;
            }
            else if (1900 <= year && year <= 1923)
            {
                day = surplus < 3 ? 21 : 22;
            }
            else if (1924 <= year && year <= 1959)
            {
                day = 21;
            }
            else if (1960 <= year && year <= 1991)
            {
                day = surplus < 1 ? 20 : 21;
            }
            else if (1992 <= year && year <= 2023)
            {
                day = surplus < 2 ? 20 : 21;
            }
            else if (2024 <= year && year <= 2055)
            {
                day = surplus < 3 ? 20 : 21;
            }
            else if (2056 <= year && year <= 2091)
            {
                day = 20;
            }
            else if (2092 <= year && year <= 2099)
            {
                day = surplus < 1 ? 19 : 20;
            }
            else if (2100 <= year && year <= 2123)
            {
                day = surplus < 1 ? 20 : 21;
            }
            else if (2124 <= year && year <= 2155)
            {
                day = surplus < 2 ? 20 : 21;
            }
            else if (2156 <= year && year <= 2187)
            {
                day = surplus < 3 ? 20 : 21;
            }
            else if (2188 <= year && year <= 2199)
            {
                day = 20;
            }
            return DateTime.Parse(date.Year + "/" + date.Month + "/" + day);
        }

        /// <summary>
        /// 秋分の日相当の日付を返す
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private DateTime getShuubun(DateTime date, int year)
        {
            int surplus = year % 4;
            int day = 23;
            if (1800 <= year && year <= 1823)
            {
                day = surplus < 2 ? 23 : 24;
            }
            else if (1824 <= year && year <= 1851)
            {
                day = surplus < 3 ? 23 : 24;
            }
            else if (1852 <= year && year <= 1887)
            {
                day = 23;
            }
            else if (1888 <= year && year <= 1899)
            {
                day = surplus < 1 ? 22 : 23;
            }
            else if (1900 <= year && year <= 1919)
            {
                day = surplus < 1 ? 23 : 24;
            }
            else if (1920 <= year && year <= 1947)
            {
                day = surplus < 2 ? 23 : 24;
            }
            else if (1948 <= year && year <= 1979)
            {
                day = surplus < 3 ? 23 : 24;
            }
            else if (1980 <= year && year <= 2011)
            {
                day = 23;
            }
            else if (2012 <= year && year <= 2043)
            {
                day = surplus < 1 ? 22 : 23;
            }
            else if (2044 <= year && year <= 2075)
            {
                day = surplus < 2 ? 22 : 23;
            }
            else if (2076 <= year && year <= 2099)
            {
                day = surplus < 3 ? 22 : 23;
            }
            else if (2100 <= year && year <= 2103)
            {
                day = surplus < 3 ? 23 : 24;
            }
            else if (2104 <= year && year <= 2139)
            {
                day = 23;
            }
            else if (2140 <= year && year <= 2167)
            {
                day = surplus < 1 ? 22 : 23;
            }
            else if (2168 <= year && year <= 2199)
            {
                day = surplus < 2 ? 22 : 23;
            }
            return DateTime.Parse(date.Year + "/" + date.Month + "/" + day);
        }

        /// <summary>
        /// 振り替え休日を求める
        /// </summary>
        /// <param name="date">基準日付</param>
        private void SetFurikae(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday && date >= this.dateFurikae)
            {
                while (this.inObject(date) || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                }
                this.setObject(date, new ItemHolidayModel(0, this.titleFurikae));
            }
        }

        /// <summary>
        /// 国民の祝日を求める
        /// </summary>
        /// <param name="date">基準日付</param>
        private void setKokumin(DateTime date)
        {
            date = date.AddDays(-2);
            if (this.inObject(date) && date >= this.dateKokumin)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek != DayOfWeek.Sunday && !this.inObject(date))
                {
                    this.setObject(date, new ItemHolidayModel(0, this.titleKokumin));
                }
            }
        }

        /// <summary>
        /// 指定されたパラメータを元に休日データを算出して一覧にセットする
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dayVal"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        private void setHoliday(int year, int month, object dayVal, string title, int type)
        {
            DateTime date = DateTime.Parse(year + "/" + month + "/" + 1);
            if (dayVal.GetType()==typeof(int))
            {
                if ((int)dayVal == Syunbun)
                {
                    date = this.getSyunbun(date, year);
                }
                else if ((int)dayVal == Shuubun)
                {
                    date = this.getShuubun(date, year);
                }
                else
                {
                    date = DateTime.Parse(year + "/" + month + "/" + (int)dayVal);
                }
            }
            else if (dayVal.GetType() == typeof(int[]))
            {
                int[] param = (int[])dayVal;
                date = this.getDayCountInMonth(date, param[0], param[1]);
            }
            else
            {
                throw new ArgumentException("日付指定が正しくありません。");
            }
            this.setObject(date, new ItemHolidayModel(type, title));
        }

        /// <summary>
        /// 休日一覧を作成し返却する
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Dictionary<string, ItemHolidayModel> getHoliday(int year)
        {
            this.holidayList.Clear();
            foreach(var item in this.holidays)
            {
                if (item.fromYear <= year && year <= item.toYear)
                {
                    this.setHoliday(year, item.month, item.days, item.title, item.type);
                }
            }

            List<string> keys = this.holidayList.Where(hl => hl.Value.type == 0).Select(hl => hl.Key).ToList();

            foreach (var item in keys)
            {
                this.SetFurikae(DateTime.Parse(item));
                this.setKokumin(DateTime.Parse(item));
            }

            return this.holidayList;
        }
    }
}
