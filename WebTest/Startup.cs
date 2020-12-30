using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SqlSugarTool;
using System.Linq;
using System.Reflection;

namespace WebTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ��services����ӵ�����container
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlSugarClient();
            services.AddControllers().AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebTest", Version = "v1" });
            });
        }

        /// <summary>
        /// ʹ�ù��ܷḻ��Autofac�������Ĭ������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder) 
        {
            //�Խӿڷ�ʽע��ִ���(ȫ��Ҫ�Խӿ���ʽע��)
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).AsImplementedInterfaces();
            //ע��Service�����µĽӿ���ʵ��
            System.Reflection.Assembly _assemblyService = Assembly.Load("Service");            
            builder.RegisterAssemblyTypes(_assemblyService).Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("I")).AsImplementedInterfaces();
            var webAssemblytype = typeof(Program).Assembly;
            builder.RegisterAssemblyTypes(webAssemblytype).PropertiesAutowired();
        }

        /// <summary>
        /// ����HTTP����ܵ�
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (name: "default", pattern: "{controller=Reception}/{action=Index}/{id?}");
                endpoints.MapControllerRoute
                ("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
}
