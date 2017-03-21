/// <reference path="../typings/tsd.d.ts" />
var holiday;
(function (holiday) {
    "use strict";
    var Holiday = (function () {
        function Holiday() {
            this.consts = {
                strFurikae: "振替休日",
                dateFurikae: new Date(1973, 3, 12),
                strKokumin: "国民の休日",
                dateKokumin: new Date(1985, 11, 27) // 国民の休日の施行日
            };
            this.fixed_holidays = [
                [2016, 12, 29, "会社休日"],
                [2016, 12, 30, "会社休日"],
                [2016, 12, 31, "会社休日"],
                [2017, 1, 2, "会社休日"],
                [2017, 1, 3, "会社休日"],
                [2017, 1, 4, "会社休日"],
            ];
            this.custom_holidays = [
                [2014, 9999, 3, 18, "手術記念"],
                [2013, 9999, 9, 25, "高山BirthDay"],
            ];
            this.holidays = [
                [1874, 1948, 1, 1, "四方節"],
                [1949, 9999, 1, 1, "元日"],
                [1874, 1948, 1, 3, "元始祭"],
                [1874, 1948, 1, 5, "新年宴会"],
                [1949, 1999, 1, 15, "成人の日"],
                [2000, 9999, 1, [2, 1], "成人の日"],
                [1874, 1912, 1, 30, "孝明天皇祭"],
                [1874, 1948, 2, 11, "紀元節"],
                [1967, 9999, 2, 11, "建国記念の日"],
                [1989, 1989, 2, 24, "昭和天皇の大喪の礼"],
                [1879, 1948, 3, this.setSyunbun, "春季皇霊祭"],
                [1949, 2199, 3, this.setSyunbun, "春分の日"],
                [1874, 1948, 4, 3, "神武天皇祭"],
                [1959, 1959, 4, 10, "皇太子・明仁親王の結婚の儀"],
                [1927, 1948, 4, 29, "天長節"],
                [1949, 1988, 4, 29, "天皇誕生日"],
                [1989, 2006, 4, 29, "みどりの日"],
                [2007, 9999, 4, 29, "昭和の日"],
                [1949, 9999, 5, 3, "憲法記念日"],
                [2007, 9999, 5, 4, "みどりの日"],
                [1949, 9999, 5, 5, "こどもの日"],
                [1993, 1993, 6, 9, "皇太子・徳仁親王の結婚の儀"],
                [1996, 2002, 7, 20, "海の日"],
                [2003, 9999, 7, [3, 1], "海の日"],
                [1913, 1926, 7, 30, "明治天皇祭"],
                [2016, 9999, 8, 11, "山の日"],
                [1913, 1926, 8, 31, "天長節"],
                [1966, 2002, 9, 15, "敬老の日"],
                [2003, 9999, 9, [3, 1], "敬老の日"],
                [1874, 1878, 9, 17, "神嘗祭"],
                [1878, 1947, 9, this.setSyuubun, "秋季皇霊祭"],
                [1948, 2199, 9, this.setSyuubun, "秋分の日"],
                [1966, 1999, 10, 10, "体育の日"],
                [2000, 9999, 10, [2, 1], "体育の日"],
                [1873, 1879, 10, 17, "神嘗祭"],
                [1913, 1926, 10, 31, "天長節祝日"],
                [1873, 1911, 11, 3, "天長節"],
                [1927, 1947, 11, 3, "明治節"],
                [1948, 9999, 11, 3, "文化の日"],
                [1990, 1990, 11, 12, "即位の礼正殿の儀"],
                [1873, 1947, 11, 23, "新嘗祭"],
                [1948, 9999, 11, 23, "勤労感謝の日"],
                [1915, 1915, 11, 10, "即位の礼"],
                [1915, 1915, 11, 14, "大嘗祭"],
                [1915, 1915, 11, 16, "大饗第1日"],
                [1928, 1928, 11, 10, "即位の礼"],
                [1928, 1928, 11, 14, "大嘗祭"],
                [1928, 1928, 11, 16, "大饗第1日"],
                [1989, 9999, 12, 23, "天皇誕生日"],
                [1927, 1947, 12, 25, "大正天皇祭"]
            ];
            this.holidayList = [];
        }
        Holiday.prototype.pad = function (val) {
            return (new Array(2).join("0") + val).slice(-2);
            // return ("0" + val).slice(-2);
        };
        Holiday.prototype.format = function (date) {
            return date.getFullYear() + "/" + this.pad(date.getMonth() + 1) + "/" + this.pad(date.getDate());
        };
        Holiday.prototype.setDayCountsInMonth = function (date, count, day) {
            var days = day - date.getDay() + 1;
            days += days < 1 ? count * 7 : (count - 1) * 7;
            date.setDate(days);
        };
        Holiday.prototype.setSyunbun = function (date, year) {
            var surplus = year % 4;
            var day = 20;
            if (1800 <= year && year <= 1827) {
                day = 21;
            }
            else if (1828 <= year && year <= 1859) {
                day = surplus < 1 ? 20 : 21;
            }
            else if (1860 <= year && year <= 1891) {
                day = surplus < 2 ? 20 : 21;
            }
            else if (1892 <= year && year <= 1899) {
                day = surplus < 3 ? 20 : 21;
            }
            else if (1900 <= year && year <= 1923) {
                day = surplus < 3 ? 21 : 22;
            }
            else if (1924 <= year && year <= 1959) {
                day = 21;
            }
            else if (1960 <= year && year <= 1991) {
                day = surplus < 1 ? 20 : 21;
            }
            else if (1992 <= year && year <= 2023) {
                day = surplus < 2 ? 20 : 21;
            }
            else if (2024 <= year && year <= 2055) {
                day = surplus < 3 ? 20 : 21;
            }
            else if (2056 <= year && year <= 2091) {
                day = 20;
            }
            else if (2092 <= year && year <= 2099) {
                day = surplus < 1 ? 19 : 20;
            }
            else if (2100 <= year && year <= 2123) {
                day = surplus < 1 ? 20 : 21;
            }
            else if (2124 <= year && year <= 2155) {
                day = surplus < 2 ? 20 : 21;
            }
            else if (2156 <= year && year <= 2187) {
                day = surplus < 3 ? 20 : 21;
            }
            else if (2188 <= year && year <= 2199) {
                day = 20;
            }
            date.setDate(day);
        };
        Holiday.prototype.setSyuubun = function (date, year) {
            var surplus = year % 4;
            var day = 23;
            if (1800 <= year && year <= 1823) {
                day = surplus < 2 ? 23 : 24;
            }
            else if (1824 <= year && year <= 1851) {
                day = surplus < 3 ? 23 : 24;
            }
            else if (1852 <= year && year <= 1887) {
                day = 23;
            }
            else if (1888 <= year && year <= 1899) {
                day = surplus < 1 ? 22 : 23;
            }
            else if (1900 <= year && year <= 1919) {
                day = surplus < 1 ? 23 : 24;
            }
            else if (1920 <= year && year <= 1947) {
                day = surplus < 2 ? 23 : 24;
            }
            else if (1948 <= year && year <= 1979) {
                day = surplus < 3 ? 23 : 24;
            }
            else if (1980 <= year && year <= 2011) {
                day = 23;
            }
            else if (2012 <= year && year <= 2043) {
                day = surplus < 1 ? 22 : 23;
            }
            else if (2044 <= year && year <= 2075) {
                day = surplus < 2 ? 22 : 23;
            }
            else if (2076 <= year && year <= 2099) {
                day = surplus < 3 ? 22 : 23;
            }
            else if (2100 <= year && year <= 2103) {
                day = surplus < 3 ? 23 : 24;
            }
            else if (2104 <= year && year <= 2139) {
                day = 23;
            }
            else if (2140 <= year && year <= 2167) {
                day = surplus < 1 ? 22 : 23;
            }
            else if (2168 <= year && year <= 2199) {
                day = surplus < 2 ? 22 : 23;
            }
            date.setDate(day);
        };
        Holiday.prototype.setFurikae = function (date) {
            if (date.getDay() === 0 && date >= this.consts.dateFurikae) {
                while (this.inObject(date) || date.getDay() === 0) {
                    date.setDate(date.getDate() + 1);
                }
                this.setObject(date, { type: 0, title: this.consts.strFurikae });
            }
        };
        Holiday.prototype.setKokumin = function (date) {
            date.setDate(date.getDate() - 2);
            if (this.inObject(date) && date >= this.consts.dateKokumin) {
                date.setDate(date.getDate() + 1);
                if (date.getDay() > 1 && !this.inObject(date)) {
                    this.setObject(date, { type: 0, title: this.consts.strKokumin });
                }
            }
        };
        Holiday.prototype.setHoliday = function (date, year, month, dateVal, name) {
            date.setFullYear(year, month, 1);
            switch (Object.prototype.toString.call(dateVal)) {
                case "[object Number]":
                    date.setDate(dateVal);
                    break;
                case "[object Array]":
                    this.setDayCountsInMonth(date, dateVal[0], dateVal[1]);
                    break;
                case "[object String]":
                    if (this.hasOwnProperty(dateVal) &&
                        Object.prototype.toString.call(this[dateVal]) === "[object Function]") {
                        dateVal(date, year);
                    }
                    else {
                        throw new Error("指定の関数が存在しません");
                    }
                    break;
                case "[object Function]":
                    dateVal(date, year);
                    break;
                default:
                    throw new Error("引数のデータ型がおかしいです");
            }
            this.setObject(date, { type: 0, title: name });
        };
        Holiday.prototype.setObject = function (date, name) {
            this.holidayList[this.format(date)] = name;
        };
        Holiday.prototype.inObject = function (date) {
            return this.holidayList.hasOwnProperty(this.format(date));
        };
        Holiday.prototype.getHoliday = function (year) {
            this.holidayList = [];
            year = parseInt(year, 10);
            var date = new Date(year, 0, 1);
            var i, len;
            for (i = 0, len = this.holidays.length; i < len; i++) {
                if (this.holidays[i][0] <= year && year <= this.holidays[i][1]) {
                    this.setHoliday(date, // Dateオブジェクト
                    year, // 年
                    this.holidays[i][2] - 1, // 月（日付のセット用に「-1」）
                    this.holidays[i][3], // 日 
                    this.holidays[i][4] // 祝祭日名
                    );
                }
            }
            var keys = Object.keys(this.holidayList).sort();
            for (i = 0, len = keys.length; i < len; i++) {
                var parse = Date.parse(keys[i] + " 00:00:00");
                date.setTime(parse);
                this.setFurikae(date);
                date.setTime(parse);
                this.setKokumin(date);
            }
            return this.holidayList;
        };
        return Holiday;
    }());
    holiday.Holiday = Holiday;
})(holiday || (holiday = {}));
//# sourceMappingURL=holidayList.js.map