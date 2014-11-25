using System;

namespace DingyuehaoZiyuan.Infrastructure
{
    public interface IRepositoryContext : IDisposable
    {
        /// <summary>提交
        /// </summary>
        void Commit();

        /// <summary>回滚
        /// </summary>
        void RollBack();
    }
}
