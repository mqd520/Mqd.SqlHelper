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
    public sealed partial class Db
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
        /// 默认存储过程返回值参数名
        /// </summary>
        private const string _SPReturnValueName = "@RETURN_VALUE";
        /// <summary>
        /// 存储过程返回值
        /// </summary>
        private object _SPReturnValue = null;

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
        /// 释放相关资源
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        private void Dispose(DbCommand cmd)
        {
            cmd.Parameters.Clear();
            if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 设置存储过程返回值
        /// </summary>
        /// <param name="collection">参数集合</param>
        private void SetSPReturnValue(DbParameterCollection collection)
        {
            if (collection != null)
            {
                DbParameter para = collection[_SPReturnValueName];
                if (para != null)
                {
                    _SPReturnValue = para.Value;
                }
            }
        }

        /// <summary>
        /// 打开指定数据库连接对象
        /// </summary>
        /// <param name="conn">连接对象</param>
        /// <returns></returns>
        private void OpenConnection(DbConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
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
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = _css.ConnectionString;
            return conn;
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
        public DbCommand CreateCommand(string cmdText, DbConnection conn,
            CommandType type = CommandType.Text, DbParameter[] paras = null, DbTransaction tran = null)
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = type;
            cmd.Connection = conn;
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
                if (cmd.CommandType == CommandType.StoredProcedure && !cmd.Parameters.Contains(_SPReturnValueName))
                {
                    cmd.Parameters.Add(CreateParameter(_SPReturnValueName, direction: ParameterDirection.ReturnValue));
                }
            }
            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            return cmd;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="type">查询类型</param>
        /// <param name="paras">参数列表</param>
        /// <returns>返回查询数据</returns>
        public DataSet Query(string sql, CommandType type = CommandType.Text, DbParameter[] paras = null)
        {
            DataSet ds = new DataSet();
            DbConnection conn = CreateConnection();
            DbCommand cmd = CreateCommand(sql, conn, type: type, paras: paras);
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            OpenConnection(conn);
            try
            {
                adapter.Fill(ds);
            }
            catch(Exception e)
            {
                Dispose(cmd);
                throw e;
            }
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                SetSPReturnValue(cmd.Parameters);
            }
            Dispose(cmd);
            return ds;
        }

        /// <summary>
        /// 执行无查询命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">命令类型</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回影响记录数</returns>
        public int ExecuteNonQuery(string sql, CommandType type = CommandType.Text, DbParameter[] paras = null)
        {
            int n = 0;
            DbConnection conn = CreateConnection();
            DbCommand cmd = CreateCommand(sql, conn, type: type, paras: paras);
            OpenConnection(conn);
            try
            {
                n = cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Dispose(cmd);
                throw e;
            }
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                SetSPReturnValue(cmd.Parameters);
            }
            Dispose(cmd);
            return n;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DbCommandBuilder CreateCmdBuild()
        {
            return _factory.CreateCommandBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DbParameter[] DeriveParameters(string objName)
        {
            DbConnection conn = CreateConnection();
            DbCommand cmd = CreateCommand(objName, conn, CommandType.StoredProcedure);
            OpenConnection(conn);
            System.Data.SqlClient.SqlCommandBuilder.DeriveParameters((System.Data.SqlClient.SqlCommand)cmd);
            if (cmd.Parameters != null)
            {
                DbParameter[] paras = new DbParameter[cmd.Parameters.Count];
                for (int i = 0; i < paras.Length; i++)
                {
                    paras[i] = cmd.Parameters[i];
                }
                Dispose(cmd);
                return paras;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取存储过程执行返回值
        /// </summary>
        public object SPReturnValue
        {
            get
            {
                return _SPReturnValue;
            }
        }
    }
}
