using System.Data.Entity;
using System.Linq;
using DingyuehaoZiyuan.Architecture;

namespace DingyuehaoZiyuan.Infrastructure
{
    /// <summary>
    /// 仓储上下文
    /// </summary>
    public class RepositoryContext : IRepositoryContext
    {
        private DbContext _dbContext = null;
        private DbAccess _dbAccess = null;

        internal DbAccess DbAccess
        {
            get { return _dbAccess ?? (_dbAccess = new DbAccess(Conntect)); }
        }

        internal DbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = new EntityFrameworkContext(Conntect)); }
        }

        public string Conntect { get; private set; }

        public RepositoryContext()
            : this("DefaultConnection")
        {
        }

        public RepositoryContext(string nameOrConnectionString)
        {
            Conntect = nameOrConnectionString;
        }

        public void Commit()
        {
            if (_dbContext != null)
            {
                _dbContext.SaveChanges();
            }
        }

        public void RollBack()
        {
            if (_dbContext != null)
            {
                _dbContext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
            }
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
