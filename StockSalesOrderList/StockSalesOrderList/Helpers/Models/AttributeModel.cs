using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttribute : Attribute
    {
        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }

        public byte Precision { get; set; }
        public byte Scale { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultSqlValueAttribute : Attribute
    {
        public DefaultSqlValueAttribute(char value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(byte value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(short value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(int value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(long value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(float value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(double value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(bool value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(string value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(object value)
        {
            Value = value;
        }
        public DefaultSqlValueAttribute(Type type, string value)
        {
            Value = value;
        }
        public object Value { get; set; }
    }
}
