using DingyuehaoZiyuan.Architecture;
using DingyuehaoZiyuan.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DingyuehaoZiyuan.Api
{
    public static class WorkFactory
    {
        static WorkFactory()
        {

        }

        public static UnitOfWork Create()
        {
            var context = new RepositoryContext("EventTracking");
            var work = UnitOfWork.CreateOnContext(context);
            return work;
        }
    }
}