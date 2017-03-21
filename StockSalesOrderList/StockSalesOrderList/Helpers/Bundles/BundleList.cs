using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace StockSalesOrderList.Helpers.Bundles
{
    public class BundleList
    {
        private bool isBaseList;
        private string virtualPath;
        private List<string> bundleFile;
        private IBundleTransform trans;

        public IBundleTransform Trans
        {
            get
            {
                return trans;
            }
        }

        public bool IsBaseList
        {
            get
            {
                return isBaseList;
            }

            set
            {
                isBaseList = value;
            }
        }

        public string VirtualPath
        {
            get
            {
                return virtualPath;
            }

            set
            {
                virtualPath = value;
            }
        }

        public List<string> BundleFile
        {
            get
            {
                return bundleFile;
            }
        }

        public void AddFile(string file)
        {
            this.bundleFile.Add(file);
            this.bundleFile = this.bundleFile.Distinct().ToList();
        }

        public void AddFiles(string[] file)
        {
            this.bundleFile.AddRange(file);
            this.bundleFile = this.bundleFile.Distinct().ToList();
        }

        public void AddFiles(List<string> file)
        {
            this.bundleFile.AddRange(file);
            this.bundleFile = this.bundleFile.Distinct().ToList();
        }

        private void Initialize(string virtualPath, BundleList prefixList, List<string> bundleList, BundleList suffixList, IBundleTransform trans = null)
        {
            this.isBaseList = false;
            this.virtualPath = virtualPath;
            this.bundleFile = new List<string>();

            if (prefixList != null)
            {
                this.bundleFile.AddRange(prefixList.BundleFile);
            }
            if (bundleList != null)
            {
                this.bundleFile.AddRange(bundleList);
            }
            if (suffixList != null)
            {
                this.bundleFile.AddRange(suffixList.BundleFile);
            }

            this.trans = trans;
        }

        public BundleList()
        {
            Initialize(string.Empty, null, null, null);
        }

        public BundleList(string virtualPath, string[] bundleList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, null, bundleList.ToList(), null, trans);
        }

        public BundleList(string virtualPath, BundleList prefixList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, prefixList, null, null, trans);
        }

        public BundleList(string virtualPath, BundleList prefixList, string[] bundleList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, prefixList, bundleList.ToList(), null, trans);
        }

        public BundleList(string virtualPath, BundleList prefixList, BundleList suffixList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, prefixList, null, suffixList, trans);
        }

        public BundleList(string virtualPath, BundleList prefixList, string[] bundleList, BundleList suffixList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, prefixList, bundleList.ToList(), suffixList, trans);
        }

        public BundleList(string virtualPath, BundleList prefixList, List<string> bundleList, BundleList suffixList, IBundleTransform trans = null)
        {
            Initialize(virtualPath, prefixList, bundleList, suffixList, trans);
        }
    }

    #region 補助クラス
    public class JsCustomMinify : JsMinify, IBundleTransform
    {
        public override void Process(BundleContext context, BundleResponse response)
        {
            base.Process(context, response);
            response.Cacheability = HttpCacheability.Private;
        }
    }

    public class CssCustomMinify : CssMinify, IBundleTransform
    {
        public override void Process(BundleContext context, BundleResponse response)
        {
            base.Process(context, response);
            response.Cacheability = HttpCacheability.Private;
        }
    }

    public class CustomMinify : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Cacheability = HttpCacheability.Private;
        }
    }
    #endregion
}
