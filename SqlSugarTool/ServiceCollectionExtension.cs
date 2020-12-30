using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace SqlSugarTool
{
    /// <summary>
    /// SQLSugar的服务扩展集（服务扩展时，只需在节点按ConnectionOption配置就会遍历生成Client）
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 支持多数据库上下文
        /// </summary>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var service = services.First(x => x.ServiceType == typeof(IConfiguration));
            //var config = (IConfiguration)service.ImplementationInstance;//3.1之前版本在项目读取IConfiguration
            var config = (IConfiguration)service.ImplementationFactory.Invoke(null);//3.1之后版本在项目启动时IConfiguration注册方式变了
            var connectOptions = config.GetSection(ConnectionOption.DbConfig).Get<List<ConnectionOption>>();

            if (connectOptions != null)
            {
                List<ConnectionConfig> list = new List<ConnectionConfig>();
                foreach (ConnectionOption option in connectOptions)
                {
                    list.Add(new ConnectionConfig
                    {
                        ConnectionString = option.ConnectionString,
                        DbType = option.DbType,
                        IsAutoCloseConnection = option.IsAutoCloseConnection,
                        InitKeyType = InitKeyType.Attribute,
                        ConfigId = option.ConfigName
                    });
                }

                //生命周期：从一次请求开始到结束期间
                if (lifetime == ServiceLifetime.Scoped)
                {
                    services.AddScoped(s => { return new SqlSugarClient(list); });
                }
                //生命周期：从服务创建到销毁期间
                if (lifetime == ServiceLifetime.Singleton)
                {
                    services.AddSingleton(s => { return new SqlSugarClient(list); });
                }
                //生命周期：每次请求瞬间
                if (lifetime == ServiceLifetime.Transient)
                {
                    services.AddSingleton(s => { return new SqlSugarClient(list); });
                }
            }

            return services;
        }
    }
}
