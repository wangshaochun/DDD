using System;
using System.ComponentModel.Composition;
using DingyuehaoZiyuan.Domain;

namespace DingyuehaoZiyuan.Application
{

    [Export(typeof(IArticleAppService))]
    public class ArticleAppService : IArticleAppService
    {
        ArticleService articleService=new ArticleService();
        public int AddArticle(ArticleData articleData)
        {
           var article=new Article {Title = articleData.Title};
           return articleService.AddArticle(article);
        }
    }
}
