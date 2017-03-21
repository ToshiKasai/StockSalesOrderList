using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace StockSalesOrderList.Models
{
    public static class ModelStateDictionaryExtensions
    {
        public static Dictionary<string, string[]> GetErrorsDictionary(
            this ModelStateDictionary modelState,
            string prefix)
        {
            return modelState.Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => String.IsNullOrWhiteSpace(kvp.Key) ? String.Empty : prefix == null ? kvp.Key : prefix.TrimEnd('.') + "." + kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
        }

        public static ModelStateDictionary GetErrors(
            this ModelStateDictionary modelState,
            string addPrefix)
        {
            Dictionary<string, string[]> list = modelState.Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => String.IsNullOrWhiteSpace(kvp.Key) ? String.Empty : addPrefix == null ? kvp.Key : addPrefix.TrimEnd('.') + "." + kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            ModelStateDictionary result = new ModelStateDictionary();

            foreach (var item in list)
            {
                foreach (var err in item.Value)
                {
                    result.AddModelError(item.Key, err);
                }
            }
            return result;
        }

        public static ModelStateDictionary GetErrorsDelprefix(
            this ModelStateDictionary modelState,
            string prefix)
        {
            Dictionary<string, string[]> list = modelState.Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => String.IsNullOrWhiteSpace(kvp.Key) ? String.Empty : prefix == null ? kvp.Key : kvp.Key.Replace(prefix.TrimEnd('.') + ".", ""),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            ModelStateDictionary result = new ModelStateDictionary();

            foreach (var item in list)
            {
                foreach (var err in item.Value)
                {
                    result.AddModelError(item.Key, err);
                }
            }
            return result;
        }
    }
}
