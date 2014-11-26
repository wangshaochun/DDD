using System;
using System.ComponentModel.Composition;
using DingyuehaoZiyuan.Domain;

namespace DingyuehaoZiyuan.Application
{

    [Export(typeof(IArticleAppService))]
    internal class ArticleAppService : ArticleService,IArticleAppService
    {
        public int AddArticle(ArticleData articleData)
        {
           //数据处理 
           var article=new Article {Title = articleData.Title};
           return base.AddArticle(article);
        }
      
    }
}
