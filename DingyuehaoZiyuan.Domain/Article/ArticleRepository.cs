using System.ComponentModel.Composition;
using DingyuehaoZiyuan.Infrastructure;

namespace DingyuehaoZiyuan.Domain
{
    [Export("ArticleRepository", typeof(IArticleRepository))]
    public class ArticleRepository : Repository<Article>, IArticleRepository { }
}
