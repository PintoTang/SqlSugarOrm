using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SqlSugarTool
{
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


        int Count(Expression<Func<T, bool>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        T GetFirst(Expression<Func<T, bool>> expression);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> expression);


        bool IsAny(Expression<Func<T, bool>> expression);
        Task<bool> IsAnyAsync(Expression<Func<T, bool>> expression);

        bool Insert(T insertObj);
        Task<bool> InsertAsync(T insertObj);
        int InsertReturnIdentity(T insertObj);
        Task<long> InsertReturnIdentityAsync(T insertObj);
        bool InsertRange(List<T> insertObjs);
        Task<bool> InsertRangeAsync(List<T> insertObjs);


        bool Delete(Expression<Func<T, bool>> expression);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
        bool Delete(T deleteObj);
        Task<bool> DeleteAsync(T deleteObj);


        bool UpdateColumns(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression);
        Task<bool> UpdateColumnsAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression);
        bool Update(T updateObj);
        Task<bool> UpdateAsync(T updateObj);
        bool UpdateRange(T[] updateObjs);
        Task<bool> UpdateRangeAsync(T[] updateObjs);


        DbResult<bool> UseTran(Action action);
        Task<DbResult<bool>> UseTranAsync(Action action);

        dynamic UseSql(string sql, SugarParameter[] param);
        dynamic UseProcedure(string proc, SugarParameter[] param);

    }
}
