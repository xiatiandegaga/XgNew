using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Linq;
using System.Reflection;

namespace Cloud.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region 自动匹配modelMap
            var mappingInterface = typeof(IEntityTypeConfiguration<>);
            //获取web当前程序集
            //var assembly = Assembly.GetEntryAssembly();
            //为了正常生成迁移文件，指定程序集名称，否则找不到
            var assembly = Assembly.Load("IdentityApi");
            var mappingTypes = assembly.GetTypes()
                .Where(x =>!x.IsAbstract  && !x.IsInterface &&!x.ContainsGenericParameters && x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType &&  y.GetGenericTypeDefinition() == mappingInterface));
            var entityMethod = typeof(ModelBuilder).GetMethods()
                .Single(x => x.Name == "Entity" && 
                        x.IsGenericMethod &&
                        x.ReturnType.Name == "EntityTypeBuilder`1");

            foreach (var mappingType in mappingTypes)
            {
                var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();
                var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);
                var entityBuilder = genericEntityMethod.Invoke(builder, null);
                var mapper = Activator.CreateInstance(mappingType);
                mapper.GetType().GetMethod("Configure").Invoke(mapper, new[] { entityBuilder });
            }
        }
        #endregion
    }
}
