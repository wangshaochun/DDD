using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DingyuehaoZiyuan.Architecture;
using DingyuehaoZiyuan.Infrastructure;

namespace DingyuehaoZiyuan.Application
{
    public static class WorkFactory
    {
        static WorkFactory()
        {

        }

        public static UnitOfWork Create()
        {
            var context = new RepositoryContext("DefaultConnection");
            var work = UnitOfWork.CreateOnContext(context);
            return work;
        }
    }
}
