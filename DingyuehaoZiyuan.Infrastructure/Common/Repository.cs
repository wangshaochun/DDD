using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DingyuehaoZiyuan.Architecture;

namespace DingyuehaoZiyuan.Infrastructure
{
    
    /// <summary>仓储基类
    /// </summary>
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>Entity Framework上下文
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return RepositoryContext.DbContext;
            }
        }

        /// <summary>仓储上下文
        /// </summary>
        public RepositoryContext RepositoryContext
        {
            get
            {
                var work = UnitOfWork.Current();
                return work.DataContext as RepositoryContext;
            }
        }

        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get { return DbContext.Set<TEntity>(); }
        }

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(TEntity entity)
        {
            if (null == entity) return 0;
            DbContext.Set<TEntity>().Add(entity);
            return DbContext.SaveChanges();
        }

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;
            DbContext.Set<TEntity>().AddRange(entities);
            return DbContext.SaveChanges();
        }

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(object id)
        {
            if (id == null) return 0;
            TEntity entity = DbContext.Set<TEntity>().Find(id);
            return Delete(entity);
        }

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(TEntity entity)
        {
            if (entity == null) return 0;
            DbContext.Set<TEntity>().Remove(entity);
            return DbContext.SaveChanges();
        }

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;
            DbContext.Set<TEntity>().RemoveRange(entities); 
            return DbContext.SaveChanges();
        }

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities = DbContext.Set<TEntity>().Where(predicate).ToList();
            return entities.Count > 0 ? Delete(entities) : 0;
        }

        /// <summary>
        ///     更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Update(TEntity entity)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State != EntityState.Detached) return 0;
            DbContext.Set<TEntity>().Attach(entity);
            entry.State = EntityState.Modified;
            return DbContext.SaveChanges();
        }

        /// <summary>
        ///     查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        public virtual TEntity GetByKey(object key)
        {
            return DbContext.Set<TEntity>().Find(key);
        }

        #endregion
    }
}
