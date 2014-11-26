using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace DingyuehaoZiyuan.Architecture
{
    /// <summary>
    /// 当前上下文
    /// </summary>
    public static class Context
    {
        static Context()
        {
        }

        /// <summary>判断当前上下文是否为Web
        /// </summary>
        public static readonly bool IsWebSite = (HttpContext.Current != null);

        /// <summary>获取缓存
        /// </summary>
        public static IDictionary Item
        {
            get
            {
                if (IsWebSite) return HttpContext.Current.Items;

                IDictionary collection = CallContext.GetData("Context_Item") as Dictionary<object, object>;
                if (collection != null) return collection;

                collection = new Dictionary<object, object>();
                CallContext.SetData("Context_Item", collection);
                return collection;
            }
        }

    }
}
