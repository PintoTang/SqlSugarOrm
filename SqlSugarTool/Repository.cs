using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SqlSugarTool
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        public SqlSugarClient DbClient { get; set; }

        //通过构造函数注入SqlSugarClient参数
        public Repository(SqlSugarClient dbClient)
        {
            DbClient = dbClient;
        }

        /// <summary>
        /// 多库切换
        /// </summary>
        /// <param name="ConfigName">数据库连接名称</param>
        /// <returns></returns>
        public IRepository<T> ChangeContext(string ConfigName)
        {
            DbClient.ChangeDatabase(ConfigName);
            return this;
        }

        public ISugarQueryable<T> AsQueryable()
        {
            return DbClient.Queryable<T>();
        }

        /// <summary>
        /// 根据id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(dynamic id)
        {
            return DbClient.Queryable<T>().InSingle(id);
        }

        public List<T> GetList()
        {
            return DbClient.Queryable<T>().ToList();
        }

        public Task<List<T>> GetListAsync()
        {
            return DbClient.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().Where(expression).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().Where(expression).ToListAsync();
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <param name="page">分页对象</param>
        /// <returns></returns>
        public List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = DbClient.Queryable<T>().Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 分页获取数据（异步）
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <param name="page">分页对象</param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = await DbClient.Queryable<T>().Where(expression).ToPageListAsync(page.PageIndex, page.PageSize, count);
            page.PageCount = result.Count;
            return result;
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().Count(expression);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().CountAsync(expression);
        }

        public T GetFirst(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().First(expression);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await DbClient.Queryable<T>().FirstAsync(expression);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public bool IsAny(Expression<Func<T, bool>> expression)
        {
            return DbClient.Queryable<T>().Any(expression);
        }

        /// <summary>
        /// 是否存在（异步）
        /// </summary>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> expression)
        {
            return await DbClient.Queryable<T>().AnyAsync(expression);
        }

        public bool Insert(T model)
        {
            return DbClient.Insertable(model).ExecuteCommand() > 0;
        }

        public async Task<bool> InsertAsync(T model)
        {
            return await DbClient.Insertable(model).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 插入并返回自增长Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertReturnIdentity(T model)
        {
            return DbClient.Insertable(model).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 插入并返回自增长Id（异步）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> InsertReturnIdentityAsync(T model)
        {
            return await DbClient.Insertable(model).ExecuteReturnBigIdentityAsync();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool InsertRange(List<T> models)
        {
            return DbClient.Insertable(models).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量插入（异步）
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T> models)
        {
            return await DbClient.Insertable(models).ExecuteCommandAsync() > 0;
        }

        public bool Delete(Expression<Func<T, bool>> expression)
        {
            return DbClient.Deleteable<T>().Where(expression).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteAsync(T model)
        {
            return await DbClient.Deleteable(model).ExecuteCommandAsync() > 0;
        }

        public bool Delete(T model)
        {
            return DbClient.Deleteable(model).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            return await DbClient.Deleteable<T>().Where(expression).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 更新对象列
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public bool UpdateColumns(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression)
        {
            return DbClient.Updateable<T>().SetColumns(columns).Where(expression).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新对象列（异步）
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="expression">Linq表达式</param>
        /// <returns></returns>
        public async Task<bool> UpdateColumnsAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression)
        {
            return await DbClient.Updateable<T>().SetColumns(columns).Where(expression).ExecuteCommandAsync() > 0;
        }

        public bool Update(T model)
        {
            return DbClient.Updateable(model).ExecuteCommand() > 0;
        }

        public async Task<bool> UpdateAsync(T model)
        {
            return await DbClient.Updateable(model).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool UpdateRange(T[] models)
        {
            return DbClient.Updateable(models).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量更新（异步）
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(T[] models)
        {
            return await DbClient.Updateable(models).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">事务Action</param>
        /// <returns></returns>
        public DbResult<bool> UseTran(Action action)
        {
            return DbClient.Ado.UseTran(action);
        }

        /// <summary>
        /// 使用事务（异步）
        /// </summary>
        /// <param name="action">事务Action</param>
        /// <returns></returns>
        public async Task<DbResult<bool>> UseTranAsync(Action action)
        {
            return await DbClient.Ado.UseTranAsync(action);
        }

        /// <summary>
        /// 使用sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic UseSql(string sql, SugarParameter[] param)
        {
            return DbClient.Ado.SqlQuery<dynamic>(sql, param);
        }

        /// <summary>
        /// 使用存储过程名称查询
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic UseProcedure(string proc, SugarParameter[] param)
        {
            return DbClient.Ado.UseStoredProcedure().SqlQuery<dynamic>(proc, param);

            //return DbClient.Ado.UseStoredProcedure().GetDataTable(proc, param);
        }

    }
}
