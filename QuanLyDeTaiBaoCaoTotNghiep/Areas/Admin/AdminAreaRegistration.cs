using System.Web.Mvc;

namespace QuanLyDeTaiBaoCaoTotNghiep.Areas.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index" ,controller="Homes", id = UrlParameter.Optional }
            );
        }
    }
}