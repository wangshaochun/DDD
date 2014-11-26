using System.ComponentModel.Composition;

namespace DingyuehaoZiyuan.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ArticleService
    {
        [Import(typeof(IArticleRepository))]
        IArticleRepository ArticleRepository { get; set; }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>业务操作结果</returns>
        public virtual int AddArticle(Article article)
        {
            //业务处理  略
            return ArticleRepository.Insert(article);
        }
    }
}
