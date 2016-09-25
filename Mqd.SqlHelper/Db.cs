using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace Mqd.SqlHelper
{
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

        public DataSet GetDataSet(string sql, DbParameter[] paras = null)
        {
            return Query(sql, paras: paras);
        }

        public DataTable GetTable(string sql, DbParameter[] paras = null)
        {
            DataSet ds = Query(sql, paras: paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        public DataTable ExecuteStoreProcedure(string procedureName, DbParameter[] paras = null)
        {
            DataSet ds = Query(procedureName, CommandType.StoredProcedure, paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

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
    }
}
