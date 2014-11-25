using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DingyuehaoZiyuan.Infrastructure
{
    public static class ModelBuilderExtension
    {
        public static void AutoBind(this DbModelBuilder modelBuilder, params Assembly[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                var modelCollection = FindAllByAttribute<TableAttribute>(assembly);
                foreach (var model in modelCollection)
                {
                    var methodInfo = typeof(DbModelBuilder).GetMethod("Entity");
                    methodInfo = methodInfo.MakeGenericMethod(model);
                    methodInfo.Invoke(modelBuilder, null);
                }
            }
        }

        public static void AutoBind(this DbModelBuilder modelBuilder, string assemblyName, bool isCheck = true)
        {
            try
            {
                modelBuilder.AutoBind(Assembly.Load(assemblyName));
            }
            catch (FileNotFoundException)
            {
                if (isCheck)
                {
                    throw;
                }
            }
        }

        public static IEnumerable<Type> FindAllByAttribute<TAttribute>(params Assembly[] assemblys) where TAttribute : Attribute
        {
            var typelist = new List<Type>();

            foreach (var assembly in assemblys)
            {
                foreach (var type in assembly.GetTypes().Where(m => m.GetCustomAttribute<TAttribute>(true) != null))
                {
                    typelist.Add(type);
                }
            }
            return typelist;
        }

    }
}
