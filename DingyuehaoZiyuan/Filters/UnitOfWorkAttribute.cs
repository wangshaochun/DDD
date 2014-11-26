using System.Web.Mvc;
using DingyuehaoZiyuan.Application;
using DingyuehaoZiyuan.Architecture;

namespace DingyuehaoZiyuan.Filters
{

    /// <summary>工作单元拦截器
    /// </summary>
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WorkFactory.Create();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                if (filterContext.Exception != null) return;
                if ((filterContext.Result as PartialViewResult) != null) return;
                if ((filterContext.Result as ViewResult) != null && !filterContext.Controller.ViewData.ModelState.IsValid) return;

                UnitOfWork.Current().Complete();
            }
            finally
            {
                UnitOfWork.Current().Close();
            }
        }

    }
}