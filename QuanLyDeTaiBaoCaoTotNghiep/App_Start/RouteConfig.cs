using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyDeTaiBaoCaoTotNghiep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Documents", action = "Index", id = UrlParameter.Optional }
            );

           // routes.MapRoute(
           //    name: "BaoCao",
           //    url: "Baocao-{id}",
           //    defaults: new { controller = "Documents", action = "XemBaoCao", id = UrlParameter.Optional },
           //    namespaces : new[]{"QuanLyDeTaiBaoCaoTotNghiep.Controller"}
           //);

        }
    }
}
