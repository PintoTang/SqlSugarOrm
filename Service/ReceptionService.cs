using Model;
using SqlSugar;
using SqlSugarTool;
using System;
using System.Data;
using System.Linq;

namespace Service
{
    public class ReceptionService: IReceptionService
    {
        private IRepository<HS_PC_TestRecord> repository;

        /// <summary>
        /// 直接注入仓储类,不必使用base来操作基类方法,这样最方便
        /// </summary>
        /// <param name="_repository"></param>
        public ReceptionService(IRepository<HS_PC_TestRecord> _repository)
        {
            repository = _repository;

            //打印SQL 
            repository.DbClient.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + repository.DbClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }


        /// <summary>
        /// 连表查询与跨库查询（不跨机器同种类型数据库）示例,跨机器查询不知能不能实现
        /// </summary>
        /// <returns></returns>
        public dynamic GetDataList()
        {
            repository.DbClient.ChangeDatabase("MYBR");

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

        /// <summary>
        /// 使用事务与多库切换示例
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 使用sql示例
        /// </summary>
        /// <returns></returns>
        public dynamic UseSqlText()
        {
            try
            {
                repository.DbClient.ChangeDatabase("MYBR");//非当前库时，需要切换
                var param = repository.DbClient.Ado.GetParameters(new { WritePeople = "190605326" });
                var result = repository.UseSql("Select SubCom,VisitStartTime from MY_BR_ReceptInfo where WritePeople=@WritePeople", param);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 调用存储过程示例
        /// </summary>
        /// <returns></returns>
        public dynamic UseStoredProcedure()
        {
            try
            {
                repository.DbClient.ChangeDatabase("MYBR");//非当前库时，需要切换
                var param = repository.DbClient.Ado.GetParameters(new { status = 1, Filter = "190605326", pageIndex = 1, pageSize = 10, rowCount = 0 });
                param[4].Direction = ParameterDirection.Output;
                repository.DbClient.Ado.IsClearParameters = false;
                var result = repository.UseProcedure("MY_BR_ReceptInfo_GetPage", param);
                repository.DbClient.Ado.IsClearParameters = true;
                var recordCount = param[4].Value;
                return result;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
