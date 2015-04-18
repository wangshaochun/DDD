using System;
using System.ComponentModel.Composition;
using DingyuehaoZiyuan.Domain;
using DingyuehaoZiyuan.DataObject;
using System.Linq;

namespace DingyuehaoZiyuan.Application
{

    [Export(typeof(IArticleAppService))]
    internal class ArticleAppService:IArticleAppService
    {

        [Import(typeof(IArticleRepository))]
        IArticleRepository ArticleRepository { get; set; }
        [Import(typeof(IAuthorRepository))]
        IAuthorRepository AuthorRepository { get; set; }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>业务操作结果</returns>
        public int AddArticle(ArticleData articleData)
        {
            if(string.IsNullOrEmpty(articleData.AuthorName))
            {
                articleData.AuthorName="Admin";
            }
            var authoritem=AuthorRepository.Entities.FirstOrDefault(a => a.AuthorName == articleData.AuthorName);
            if(authoritem==null)
            {
                AuthorRepository.Insert(new Author() { AuthorID = Guid.NewGuid(), AuthorName = articleData.AuthorName });
                authoritem = AuthorRepository.Entities.FirstOrDefault(a => a.AuthorName == articleData.AuthorName);
            }
            var article = new Article { Id = Guid.NewGuid(), Title = articleData.Title, AuthorID = authoritem.AuthorID };
            return ArticleRepository.Insert(article);
        }
      
    }
}
