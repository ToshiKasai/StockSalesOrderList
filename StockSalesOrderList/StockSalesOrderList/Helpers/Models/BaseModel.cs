using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                foreach (Attribute attr in p.GetCustomAttributes(true))
                {
                    if (attr is DefaultValueAttribute)
                    {
                        DefaultValueAttribute dv = (DefaultValueAttribute)attr;
                        try
                        {
                            if (p.PropertyType.Name.Contains("Nullable"))
                            {
                                p.SetValue(this, dv.Value == null ? dv.Value : Convert.ChangeType(dv.Value, p.PropertyType.GenericTypeArguments[0]));
                            }
                            else
                            {
                                p.SetValue(this, Convert.ChangeType(dv.Value, p.PropertyType));
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}
