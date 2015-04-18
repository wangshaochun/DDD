using System.ComponentModel.Composition;
using System.Web.Mvc;
using DingyuehaoZiyuan.Application;
using DingyuehaoZiyuan.Domain;
using DingyuehaoZiyuan.DataObject;

namespace DingyuehaoZiyuan.Controllers
{
    [Export]
    public class ArticleController : Controller
    {
        //
        // GET: /Article/

        #region 属性

        [Import(typeof(IArticleAppService))]
        IArticleAppService ArticleAppService { get; set; }


        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                 ArticleAppService.AddArticle(new ArticleData{Title = "测试数据Title"});
            }
            return View();
        }
    }
}
