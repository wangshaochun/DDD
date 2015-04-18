using DingyuehaoZiyuan.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DingyuehaoZiyuan.Api.Filters
{

    /// <summary>工作单元拦截器
    /// </summary>
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            WorkFactory.Create();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                UnitOfWork.Current().Complete();
            }
            finally
            {
                UnitOfWork.Current().Close();
            }
        }

    }
}