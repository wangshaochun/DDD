using System;
using System.Collections.Generic;

namespace DingyuehaoZiyuan.Architecture
{
    /// <summary>工作单元
    /// </summary>
    public sealed class UnitOfWork : IDisposable
    {
        private readonly IRepositoryContext _dataContexts;

        /// <summary>工作单元的字典
        /// </summary>
        public Dictionary<string, object> Item { get; set; }

        /// <summary>构造函数
        /// </summary>
        /// <param name="dataContexts">数据上下文</param>
        private UnitOfWork(IRepositoryContext dataContexts = null)
        {
            _dataContexts = dataContexts;
            Item = new Dictionary<string, object>();
            _dataContexts = dataContexts;
        }

        /// <summary>在当前上下文创建工作
        /// </summary>
        /// <param name="dataContexts">数据上下文</param>
        /// <returns>返回当前工作</returns>
        public static UnitOfWork CreateOnContext(IRepositoryContext dataContexts)
        {
            if (Context.Item["UnitOfWork"] != null)
            {
                throw new Exception("当前上下文中已经创建工作");
            }

            var work = new UnitOfWork(dataContexts);
            Context.Item["UnitOfWork"] = work;
            return work;
        }

        /// <summary>获取当前的工作单元
        /// </summary>
        public static UnitOfWork Current()
        {
            var work = Context.Item["UnitOfWork"] as UnitOfWork;
            if (work == null) throw new Exception("当前上下文未创建Work！");
            return work;
        }

        /// <summary>工作完成
        /// </summary>
        public void Complete()
        {
            Save();
        }

        /// <summary>工作存档
        /// </summary>
        public void Save()
        {
            if (_dataContexts != null)
            {
                _dataContexts.Commit();
            }
        }

        /// <summary>工作结束
        /// </summary>
        public void Close()
        {
            if (_dataContexts != null)
            {
                _dataContexts.Dispose();
            }
            Item.Clear();
        }

        /// <summary>释放资源
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>数据上下文
        /// </summary>
        public IRepositoryContext DataContext
        {
            get
            {
                return _dataContexts;
            }
        }
    }
}
