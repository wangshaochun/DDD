using DingyuehaoZiyuan.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingyuehaoZiyuan.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            LogHelper.WriteLog("filterContext异常拦截：", filterContext.Exception);
        }
    }
}