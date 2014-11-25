using System.ComponentModel.Composition;

namespace DingyuehaoZiyuan.Domain
{
  
    public class ArticleService
    {
        //[Import("ArticleRepository", typeof(IArticleRepository))]
        //IArticleRepository ArticleRepository { get; set; }
        IArticleRepository articleRepository=new ArticleRepository();
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>业务操作结果</returns>
        public int AddArticle(Article article)
        {
            return articleRepository.Insert(article);
        }
    }
}
