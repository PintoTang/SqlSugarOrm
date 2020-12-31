### .Net5.0 封装的SqlSugar Orm工具（已带WebTest测试项目）
 
 欢迎各位使用与指正!!!
	
 
1.在ServiceCollectionExtension.cs类扩展服务集，返回SqlSugarClient客户端（支持多数据库上下文）

2.配置节点如下：

	"DbConfig": {
    "MYBR": {
      "ConfigName": "MYBR",
      "ConnectionString": "Database=MY_BR;Server=localhost;User ID=test;Password=test;Max Pool Size = 512;",
      "DbType": "SQLServer",
      "IsAutoCloseConnection": true,
      "Default": true
    },
    "MYHR": {
      "ConfigName": "MYHR",
      "ConnectionString": "Database=MY_HR_2020061101;Server=localhost;User ID=test;Password=test;Max Pool Size = 512;",
      "DbType": "SQLServer",
      "IsAutoCloseConnection": true,
      "Default": true
    },
    "HSPC": {
      "ConfigName": "HSPC",
      "ConnectionString": "server=localhost;database=humituresensor;userid=root;password=123456",
      "DbType": "MySql",
      "IsAutoCloseConnection": true,
      "Default": true
    }
  	}		
  
  3.需要使用这个扩展服务时，在startup.cs类中
		
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlSugarClient();//在此添加
            services.AddControllers().AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebTest", Version = "v1" });
            });
        }
								
   4.使用Autofac替代默认容器        
			
        public void ConfigureContainer(ContainerBuilder builder) 
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).AsImplementedInterfaces();//注册泛型仓储类
            System.Reflection.Assembly _assemblyService = Assembly.Load("Service");            
            builder.RegisterAssemblyTypes(_assemblyService).Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("I")).AsImplementedInterfaces();
            var webAssemblytype = typeof(Program).Assembly;
            builder.RegisterAssemblyTypes(webAssemblytype).PropertiesAutowired();
        }
        
  5.服务层使用构造函数注入泛型仓储
  
        private IRepository<TestRecord> repository;
        public ReceptionService(IRepository<TestRecord> _repository)//用接口注入
        {
            repository = _repository;
            repository.DbClient.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + repository.DbClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
   6. 多表查询与跨库查询（不跨机器同种类型数据库）示例,跨机器查询不知能不能实现
			
			public dynamic GetDataList()
			{
					repository.DbClient.ChangeDatabase("MYBR");//切换数据库上下文
					var test2 = repository.DbClient.Queryable<MY_BR_ReceptInfo, MY_HR_UserInfo>((t, u) => new object[] 
					{JoinType.Left,t.WritePeople==u.JobNo})
					.AS<MY_BR_ReceptInfo>("MY_BR.dbo.MY_BR_ReceptInfo")
					.AS<MY_HR_UserInfo>("MY_HR_2020061101.dbo.MY_HR_UserInfo")
					.Select((t, u) => new
					{
					子公司 = t.SubCom,
					报备人 = u.JobNo
					})
					.ToList();
					return test2;
			}
			
   7.使用事务与多库切换示例

		public int UseTranExample()
		{
			try
			{
				var result = repository.UseTran(()=>
				{
				var uuu = repository.DbClient.Ado.ExecuteCommand("Update TestRecord set Remark='Test222' where Remark='Test111'");
				repository.DbClient.ChangeDatabase("MYHR");//切换数据库“MYHR”
				var bbb = repository.DbClient.Queryable<MY_HR_UserInfo>().Where(u => u.JobNo == "190605326").ToList();//使用“MYHR”库   
				});
				if (result.IsSuccess)
					return 1;
				else
					return 0;
			}
			catch
			{
				return 0;
			}
		}
        
   8.泛型仓库类主要封装了常用的一些方法
			
     public interface IRepository<T> where T :class,new ()
     {
        SqlSugarClient DbClient { get; }
        IRepository<T> ChangeContext(string ConfigName);
        ISugarQueryable<T> AsQueryable();
        List<T> GetList();
        Task<List<T>> GetListAsync();
        List<T> GetList(Expression<Func<T, bool>> expression);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);        
        List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page);
        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> expression, PageModel page);
        ......
      }
