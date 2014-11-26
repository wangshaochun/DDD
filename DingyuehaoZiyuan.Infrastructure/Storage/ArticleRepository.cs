using System.ComponentModel.Composition;
using DingyuehaoZiyuan.Domain;

namespace DingyuehaoZiyuan.Infrastructure.Storage
{
    [Export(typeof(IArticleRepository))]
    internal class ArticleRepository : Repository<Article>, IArticleRepository { }
}
