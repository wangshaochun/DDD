using DingyuehaoZiyuan.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace DingyuehaoZiyuan.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogHelper.WriteLog("API 发生异常", actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}