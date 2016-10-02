using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public sealed class Db
    {
        /// <summary>
        /// 数据库连接字符串配置对象
        /// </summary>
        private readonly ConnectionStringSettings _css;

        /// <summary>
        /// db提供程序对象工厂
        /// </summary>
        private readonly DbProviderFactory _factory;

        /// <summary>
        /// 使用指定数据库连接对象创建数据库连接
        /// </summary>
        /// <param name="css">数据库连接对象</param>
        public Db(ConnectionStringSettings css)
        {
            _css = css;
            _factory = DbProviderFactories.GetFactory(css.ProviderName);
        }

        /// <summary>
        /// 使用指定数据库连接字符串名创建数据库连接
        /// </summary>
        /// <param name="connStringName">指定数据库连接字符串名</param>
        public Db(string connStringName)
            : this(ConfigurationManager.ConnectionStrings[connStringName])
        { }

        /// <summary>
        /// 使用默认连接字符串名SqlHelperConnName创建数据库连接
        /// </summary>
        public Db()
            : this("SqlHelperConnName")
        { }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateConnection()
        {
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = _css.ConnectionString;
            return conn;
        }

        /// <summary>
        /// 关闭连接对象
        /// </summary>
        private void CloseConnection(DbConnection conn)
        {
            conn.Close();
        }

        /// <summary>
        /// 创建DbCommand
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="conn"></param>
        /// <param name="type"></param>
        /// <param name="paras"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(string cmdText, DbConnection conn,
            CommandType type = CommandType.Text, DbParameter[] paras = null, DbTransaction tran = null)
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = type;
            cmd.Connection = conn;
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
            }
            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            return cmd;
        }

        private DataSet Query(string sql, CommandType type = CommandType.Text, DbParameter[] paras = null)
        {
            DataSet ds = new DataSet();
            using (DbConnection conn = CreateConnection())
            {
                DbCommand cmd = CreateCommand(sql, conn, type: type, paras: paras);
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                try
                {
                    conn.Open();
                    adapter.Fill(ds);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 创建DbParameter
        /// </summary>
        /// <returns>DbParameter</returns>
        public DbParameter CreateParameter()
        {
            return _factory.CreateParameter();
        }

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
            para.Size = 0;
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
        public DataTable ExecuteStoreProcedure(string procedureName, DbParameter[] paras = null)
        {
            DataSet ds = Query(procedureName, CommandType.StoredProcedure, paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        /// <summary>
        /// 执行无查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回影响记录数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] paras = null)
        {
            int n = 0;
            using (DbConnection conn = CreateConnection())
            {
                DbCommand cmd = CreateCommand(sql, conn, paras: paras);
                try
                {
                    conn.Open();
                    n = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <param name="where">需要插入数据的字段</param>
        /// <returns>返回影响记录数</returns>
        public int Insert<T>(T entity, List<PropertyInfo> where = null)
        {
            Type t = entity.GetType();
            PropertyInfo[] pis = t.GetProperties();
            List<DbParameter> paras = new List<DbParameter>();
            string fields = "";
            string values = "";
            foreach (var item in pis)
            {
                bool flag = false;
                object o = item.GetValue(entity);
                if (o != null)
                {
                    if (where == null)
                    {
                        flag = true;
                    }
                    else if (where.Contains(item))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    fields += string.Format(",{0}", item.Name);
                    values += string.Format(",@{0}", item.Name);
                    DbParameter para = CreateParameter();
                    para.ParameterName = "@" + item.Name;
                    para.Value = o;
                    paras.Add(para);
                }
            }
            string sql = string.Format("insert into {0}({1}) values({2})", t.Name, fields.Substring(1), values.Substring(1));
            return ExecuteNonQuery(sql, paras.ToArray());
        }
    }
}
