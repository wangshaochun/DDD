using DingyuehaoZiyuan.DataObject;
using DingyuehaoZiyuan.Domain;

namespace DingyuehaoZiyuan.Application
{
    public interface IArticleAppService
    {
        int AddArticle(ArticleData article);
    }
}
