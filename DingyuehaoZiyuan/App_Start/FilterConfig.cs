using System.Web;
using System.Web.Mvc;
using DingyuehaoZiyuan.Filters;

namespace DingyuehaoZiyuan
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new UnitOfWorkAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}