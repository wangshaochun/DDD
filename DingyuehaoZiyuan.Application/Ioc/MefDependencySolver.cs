using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Web;
using System.Web.Mvc;

namespace DingyuehaoZiyuan.Application
{
    public class MefDependencySolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly ComposablePartCatalog _catalog;
        private const string MefContainerKey = "MefContainerKey";

        public MefDependencySolver(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        public CompositionContainer Container
        {
            get
            {
                return new CompositionContainer(_catalog);
                //引发error:不能使用控制器“ ”的单个实例处理多个请求。如果正在使用自定义控制器工厂，请确保它为每个请求创建该控制器的新实例。
                //if (HttpContext.Current.Cache.Get(MefContainerKey)==null)
                //{
                //    HttpContext.Current.Cache.Insert(MefContainerKey, new CompositionContainer(_catalog));
                //} 
                //var container = HttpContext.Current.Cache.Get(MefContainerKey) as CompositionContainer;               
                //return container;
            }
        }


        public object GetService(Type serviceType)
        {
            string contractName = AttributedModelServices.GetContractName(serviceType);
            return Container.GetExportedValueOrDefault<object>(contractName);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetExportedValues<object>(serviceType.FullName);
        }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}