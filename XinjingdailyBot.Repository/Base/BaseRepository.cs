﻿using System.Data;
using System.Linq.Expressions;
using SqlSugar;
using SqlSugar.IOC;
using XinjingdailyBot.Model.Base;

namespace XinjingdailyBot.Repository.Base
{
    /// <summary>
    /// 仓储层基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : BaseModel, new()
    {
        public BaseRepository(ISqlSugarClient? context = null) : base(context)
        {
            Context = context ?? DbScoped.SugarScope.GetConnectionScope(0);//没有默认db0
        }

        #region add

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Add(T t)
        {
            return Context.CopyNew().Insertable(t).IgnoreColumns(true).ExecuteCommand();
        }

        public int Insert(List<T> t)
        {
            return Context.CopyNew().Insertable(t).ExecuteCommand();
        }
        public int Insert(T parm, Expression<Func<T, object>>? iClumns = null, bool ignoreNull = true)
        {
            return Context.CopyNew().Insertable(parm).InsertColumns(iClumns).IgnoreColumns(ignoreNullColumn: ignoreNull).ExecuteCommand();
        }
        public IInsertable<T> Insertable(T t)
        {
            return Context.CopyNew().Insertable<T>(t);
        }
        #endregion add

        #region update
        public IUpdateable<T> Updateable(T entity)
        {
            return Context.CopyNew().Updateable(entity);
        }
        public int Update(T entity, bool ignoreNullColumns = false)
        {
            return Context.CopyNew().Updateable(entity).IgnoreColumns(ignoreNullColumns).ExecuteCommand();
        }

        public int Update(T entity, Expression<Func<T, object>> expression, bool ignoreAllNull = false)
        {
            return Context.CopyNew().Updateable(entity).UpdateColumns(expression).IgnoreColumns(ignoreAllNull).ExecuteCommand();
        }

        /// <summary>
        /// 根据实体类更新指定列 eg：Update(dept, it => new { it.Status }, f => depts.Contains(f.DeptId));只更新Status列，条件是包含
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expression"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(T entity, Expression<Func<T, object>> expression, Expression<Func<T, bool>> where)
        {
            return Context.CopyNew().Updateable(entity).UpdateColumns(expression).Where(where).ExecuteCommand();
        }

        public int Update(SqlSugarClient client, T entity, Expression<Func<T, object>> expression, Expression<Func<T, bool>> where)
        {
            return client.CopyNew().Updateable(entity).UpdateColumns(expression).Where(where).ExecuteCommand();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <param name="isNull">默认为true</param>
        /// <returns></returns>
        public int Update(T entity, List<string>? list = null, bool isNull = true)
        {
            list ??= new List<string>
            {
                "Create_By",
                "Create_time"
            };

            return Context.CopyNew().Updateable(entity).IgnoreColumns(isNull).IgnoreColumns(list.ToArray()).ExecuteCommand();
        }

        /// <summary>
        /// 更新指定列 eg：Update(w => w.NoticeId == model.NoticeId, it => new SysNotice(){ Update_time = DateTime.Now, Title = "通知标题" });
        /// </summary>
        /// <param name="where"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> columns)
        {
            return Context.CopyNew().Updateable<T>().SetColumns(columns).Where(where).RemoveDataCache().ExecuteCommand();
        }
        #endregion update

        public DbResult<bool> UseTran(Action action)
        {
            try
            {
                var result = Context.CopyNew().Ado.UseTran(() => action());
                return result;
            }
            catch (Exception ex)
            {
                Context.CopyNew().Ado.RollbackTran();
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public IStorageable<T> Storageable(T t)
        {
            return Context.CopyNew().Storageable<T>(t);
        }
        public IStorageable<T> Storageable(List<T> t)
        {
            return Context.CopyNew().Storageable(t);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="action">增删改查方法</param>
        /// <returns></returns>
        public DbResult<bool> UseTran(SqlSugarClient client, Action action)
        {
            try
            {
                var result = client.CopyNew().AsTenant().UseTran(() => action());
                return result;
            }
            catch (Exception ex)
            {
                client.CopyNew().AsTenant().RollbackTran();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public bool UseTran2(Action action)
        {
            var result = Context.CopyNew().Ado.UseTran(() => action());
            return result.IsSuccess;
        }

        #region delete
        public IDeleteable<T> Deleteable()
        {
            return Context.CopyNew().Deleteable<T>();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Delete(object[] obj)
        {
            return Context.CopyNew().Deleteable<T>().In(obj).ExecuteCommand();
        }
        public int Delete(object id)
        {
            return Context.CopyNew().Deleteable<T>(id).ExecuteCommand();
        }
        public int DeleteTable()
        {
            return Context.CopyNew().Deleteable<T>().ExecuteCommand();
        }
        public bool Truncate()
        {
            return Context.CopyNew().DbMaintenance.TruncateTable<T>();
        }
        #endregion delete

        #region query

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return Context.CopyNew().Queryable<T>().Where(expression).Any();
        }

        public ISugarQueryable<T> Queryable()
        {
            return Context.CopyNew().Queryable<T>();
        }

        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, int pageIndex = 0, int pageSize = 10)
        {
            int totalNumber = 0;
            var list = Context.CopyNew().Queryable<T>().Where(expression).ToPageList(pageIndex, pageSize, ref totalNumber);
            return (list, totalNumber);
        }

        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, string order, int pageIndex = 0, int pageSize = 10)
        {
            int totalNumber = 0;
            var list = Context.CopyNew().Queryable<T>().Where(expression).OrderBy(order).ToPageList(pageIndex, pageSize, ref totalNumber);
            return (list, totalNumber);
        }

        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderFiled, string orderBy, int pageIndex = 0, int pageSize = 10)
        {
            int totalNumber = 0;

            if (orderBy.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            {
                var list = Context.CopyNew().Queryable<T>().Where(expression).OrderBy(orderFiled, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
            else
            {
                var list = Context.CopyNew().Queryable<T>().Where(expression).OrderBy(orderFiled, OrderByType.Asc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
        }

        public List<T> SqlQueryToList(string sql, object? obj = null)
        {
            return Context.CopyNew().Ado.SqlQuery<T>(sql, obj);
        }

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public T GetId(object pkValue)
        {
            return Context.CopyNew().Queryable<T>().InSingle(pkValue);
        }
        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<T> GetPages(Expression<Func<T, bool>> where, PagerInfo parm)
        {
            var source = Context.CopyNew().Queryable<T>().Where(where);

            return source.ToPage(parm);
        }

        public PagedInfo<T> GetPages(Expression<Func<T, bool>> where, PagerInfo parm, Expression<Func<T, object>> order, OrderByType orderEnum = OrderByType.Asc)
        {
            var source = Context.CopyNew().Queryable<T>().Where(where).OrderByIF(orderEnum == OrderByType.Asc, order, OrderByType.Asc).OrderByIF(orderEnum == OrderByType.Desc, order, OrderByType.Desc);

            return source.ToPage(parm);
        }

        public PagedInfo<T> GetPages(Expression<Func<T, bool>> where, PagerInfo parm, Expression<Func<T, object>> order, string orderByType)
        {
            return GetPages(where, parm, order, orderByType == "desc" ? OrderByType.Desc : OrderByType.Asc);
        }

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll(bool useCache = false, int cacheSecond = 3600)
        {
            return Context.CopyNew().Queryable<T>().WithCacheIF(useCache, cacheSecond).ToList();
        }

        #endregion query

        /// <summary>
        /// 此方法不带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable UseStoredProcedureToDataTable(string procedureName, List<SugarParameter> parameters)
        {
            return Context.CopyNew().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue, true));  output
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public (DataTable, List<SugarParameter>) UseStoredProcedureToTuple(string procedureName, List<SugarParameter> parameters)
        {
            var result = (Context.CopyNew().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters), parameters);
            return result;
        }
    }

    /// <summary>
    /// 分页查询扩展
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">查询表单式</param>
        /// <param name="parm">分页参数</param>
        /// <returns></returns>
        public static PagedInfo<T> ToPage<T>(this ISugarQueryable<T> source, PagerInfo parm)
        {
            var page = new PagedInfo<T>();
            var total = 0;
            page.PageSize = parm.PageSize;
            page.PageIndex = parm.PageNum;

            page.Result = source.OrderByIF(!string.IsNullOrEmpty(parm.Sort), $"{parm.Sort} {(parm.SortType.Contains("desc") ? "desc" : "asc")}")
                .ToPageList(parm.PageNum, parm.PageSize, ref total);
            page.TotalNum = total;
            return page;
        }

    }
}
