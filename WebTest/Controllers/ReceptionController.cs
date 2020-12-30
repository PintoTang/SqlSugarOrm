using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebTest
{
    [Route("api/[controller]/[action]")]
    public class ReceptionController : Controller
    {
        public IReceptionService service { get; set; }

        public ReceptionController ()
        {

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>返回json结果</returns>
        [HttpGet]
        public JsonResult Index()
        {
            var test = service.GetDataList();
            return Json(test);
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <returns>返回成功与否</returns>
        [HttpPut]
        public OkObjectResult UseTran()
        {
            int result = service.UseTranExample();
            if (result == 1)
                return Ok(result);
            else
                return Ok(result);
        }

        /// <summary>
        /// 使用sql语句
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UseSql()
        {
            var test = service.UseSqlText();
            return Json(test);
        }

        /// <summary>
        /// 使用存储过程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UseProcedure()
        {
            var test = service.UseStoredProcedure();
            return Json(test);
        }
    }
}
