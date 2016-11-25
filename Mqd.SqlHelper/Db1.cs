using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Db
    {
        /// <summary>
        /// 创建DbParameter
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="size">参数大小</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数方向</param>
        /// <returns>DbParameter</returns>
        public DbParameter CreateParameter(string paraName, object value = null, int size = 0, DbType type = (DbType)(-1),
            ParameterDirection direction = ParameterDirection.Input)
        {
            DbParameter para = _factory.CreateParameter();
            para.ParameterName = paraName;
            para.Direction = direction;
            para.Size = size;
            para.Value = value;
            if ((int)type != -1)
            {
                para.DbType = type;
            }
            return para;
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回查询到的数据</returns>
        public DataSet GetDataSet(string sql, DbParameter[] paras = null)
        {
            return Query(sql, paras: paras);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string sql, DbParameter[] paras = null)
        {
            DataSet ds = Query(sql, paras: paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回查询到的数据</returns>
        public DataTable ExecStoreProcedure(string procedureName, DbParameter[] paras = null)
        {
            DataSet ds = Query(procedureName, CommandType.StoredProcedure, paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        /// <summary>
        /// 执行无查询存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public void ExecNoQueryStoreProcedure(string procedureName, DbParameter[] paras = null)
        {
            ExecuteNonQuery(procedureName, type: CommandType.StoredProcedure, paras: paras);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <param name="where">筛选需要插入的字段</param>
        /// <returns>返回受影响的记录条数</returns>
        public int Insert<T>(T entity, Func<PropertyInfo, bool> where = null)
        {
            Type t = entity.GetType();
            PropertyInfo[] pis = t.GetProperties();
            List<PropertyInfo> pResult = new List<PropertyInfo>();//属性筛选结果
            List<DbParameter> paras = new List<DbParameter>();
            string fields = "";
            string values = "";
            if (where != null)
            {
                foreach (var item in pis)
                {
                    bool flag = where.Invoke(item);
                    if (flag)
                    {
                        pResult.Add(item);
                    }
                }
            }
            else
            {
                pResult = pis.ToList();
            }
            foreach (var item in pResult)
            {
                object o = item.GetValue(entity);
                fields += string.Format(",[{0}]", item.Name);
                values += string.Format(",@{0}", item.Name);
                DbParameter para = CreateParameter();
                para.ParameterName = "@" + item.Name;
                para.Value = o;
                paras.Add(para);
            }
            string sql = string.Format("insert into [{0}]({1}) values({2})", t.Name, fields.Substring(1), values.Substring(1));
            return ExecuteNonQuery(sql, paras: paras.ToArray());
        }
    }
}
