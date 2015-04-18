using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace DingyuehaoZiyuan.Infrastructure
{
    /// <summary>EntityFramework 上下文
    /// </summary>
    public class EntityFrameworkContext : DbContext
    {
        /// <summary>构造函数
        /// </summary>
        /// <param name="nameOrConnectionString">链接串</param>
        public EntityFrameworkContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EntityFrameworkContext>());
            //Configuration.ValidateOnSaveEnabled = false;
            Database.Log = message => Debug.WriteLine(message);
        }

        /// <summary>当模型创建是触发的方法
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(RepositoryContext).Assembly);
            modelBuilder.AutoBind("DingyuehaoZiyuan.Domain", true);
            base.OnModelCreating(modelBuilder);
        }
    }
}
