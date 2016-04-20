using DSCMS.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace DSCMS.Web.Helper
{
    public class GenericGridCall<T> where T : class
    {
        //Get Data From Database By Passing name of table and other parameters
        public static PagedData<T> ListView(int PageSize, Func<T, object> orderByColumn, string filter = null, string orderBy = null, int orderByAsc = 1, int page = 1)
        {
            page = page == 0 ? 1 : page;
            PageSize = PageSize == 0 ? 1 : PageSize;

            Dictionary<string, string> values = null;
            if (filter != null)
            {
                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
            }

            DbSet<T> dbSet;
            DS_CMS_DB_Entities dbContext = new DS_CMS_DB_Entities();
            dbSet = dbContext.Set<T>();
            IQueryable<T> query = dbSet;
            var listObj = new PagedData<T>();
            listObj.CurrentPage = page;
            if (filter != null)
            {
                query = QueryExtensions.Filter<T>(query, values);
            }
            var data = orderByColumn;
            if (orderBy != null)
            {
                data = QueryExtensions.GetOrderByExpression<T>(orderBy);
            }
            if (PageSize == 101)
            {
                if (orderByAsc == 1)
                    listObj.Data = query.OrderBy(data).ToList();
                else
                    listObj.Data = query.OrderByDescending(data).ToList();
            }
            else
            {
                if (orderByAsc == 1)
                    listObj.Data = query.OrderBy(data).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                else
                    listObj.Data = query.OrderByDescending(data).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            }
            listObj.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)query.Count() / PageSize));
            return listObj;
        }
        
        public static PagedData<T> ListView(int PageSize, Func<T, object> orderByColumn, Func<T, bool> filterCondition = null, string filter = null, string orderBy = null, int orderByAsc = 1, int page = 1)
        {
            page = page == 0 ? 1 : page;
            PageSize = PageSize == 0 ? 1 : PageSize;

            Dictionary<string, string> values = null;
            if (filter != null)
            {
                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
            }

            DbSet<T> dbSet;
            DS_CMS_DB_Entities dbContext = new DS_CMS_DB_Entities();
            dbSet = dbContext.Set<T>();
            IQueryable<T> query = dbSet;
            var listObj = new PagedData<T>();
            listObj.CurrentPage = page;
            if (filter != null)
            {
                query = QueryExtensions.Filter<T>(query, values);
            }
            var data = orderByColumn;
            if (orderBy != null)
            {
                if (!orderBy.Contains("."))
                    data = QueryExtensions.GetOrderByExpression<T>(orderBy);
            }
            if (filterCondition != null)
            {
                query = query.Where(filterCondition).AsQueryable();
            }
            if (PageSize == 101)
            {
                if (orderByAsc == 1)
                    listObj.Data = query.OrderBy(data).ToList();
                else
                    listObj.Data = query.OrderByDescending(data).ToList();
            }
            else
            {
                if (orderByAsc == 1)
                {
                    if (orderBy != null && orderBy.Contains('.'))
                    {
                        Assembly assembly = Assembly.GetAssembly(typeof(T));
                        Type typeData = typeof(T);
                        var typeDataTemp = typeData.GetProperties();
                        var propertiesDetails = orderBy.Split('.');
                        Type t = (dynamic)null;

                        for (int i = 0; i <= propertiesDetails.Length - 1; i++)
                        {
                            foreach (PropertyInfo prope in typeDataTemp)
                            {
                                if (prope.Name.ToLower() == propertiesDetails[i].ToLower() && prope.PropertyType.IsClass)
                                {
                                    if (assembly.GetType(((prope.PropertyType)).FullName) != null)
                                    {
                                        typeDataTemp = assembly.GetType(((prope.PropertyType)).FullName).GetProperties();
                                        break;
                                    }
                                }
                                else if (prope.Name.ToLower() == propertiesDetails[i].ToLower())
                                {
                                    t = prope.PropertyType;
                                }
                            }
                        }


                        try
                        {
                            if (t.Name.ToLower() == "boolean")
                                listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, bool>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("datetime"))
                                listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, DateTime>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("decimal"))
                                listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, decimal>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("int64"))
                                listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, long>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("int"))
                                listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, int>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                        }
                        catch
                        {
                            listObj.Data = query.OrderBy(QueryExtensions.GetExpression<T, Object>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                        }
                    }
                    else
                    {
                        listObj.Data = query.OrderBy(data).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                    }
                }
                else
                {
                    if (orderBy != null && orderBy.Contains('.'))
                    {

                        Assembly assembly = Assembly.GetAssembly(typeof(T));
                        Type typeData = typeof(T);
                        var typeDataTemp = typeData.GetProperties();
                        var propertiesDetails = orderBy.Split('.');
                        Type t = (dynamic)null;

                        for (int i = 0; i <= propertiesDetails.Length - 1; i++)
                        {
                            foreach (PropertyInfo prope in typeDataTemp)
                            {
                                if (prope.Name.ToLower() == propertiesDetails[i].ToLower() && prope.PropertyType.IsClass)
                                {
                                    if (assembly.GetType(((prope.PropertyType)).FullName) != null)
                                    {
                                        typeDataTemp = assembly.GetType(((prope.PropertyType)).FullName).GetProperties();
                                    }
                                    break;
                                }
                                else if (prope.Name.ToLower() == propertiesDetails[i].ToLower())
                                {
                                    t = prope.PropertyType;
                                }
                            }
                        }


                        try
                        {
                            if (t.Name.ToLower() == "boolean")
                                listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, bool>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("datetime"))
                                listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, DateTime>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("decimal"))
                                listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, decimal>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("int64"))
                                listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, long>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                            else if (t.GenericTypeArguments[0].Name.ToLower().Contains("int"))
                                listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, int>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                        }
                        catch
                        {
                            listObj.Data = query.OrderByDescending(QueryExtensions.GetExpression<T, Object>(orderBy)).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                        }
                    }
                    else
                    {
                        listObj.Data = query.OrderByDescending(data).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                    }

                }
            }
            listObj.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)query.Count() / PageSize));
            return listObj;
        }
    }

    public static class Pager
    {
        //if page are more then 5 custom pager functionality will be call automatically
        public static int PagerStart = 5;
    }
}
