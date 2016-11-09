using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 数据访问通用处理
    /// </summary>
    internal class DbAccess : IDbAccess
    {
        /// <summary>
        /// 获取指定表所有记录
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns></returns>
        public virtual DataTable GetAll(string table)
        {
            return new DataTable();
        }
    }
}
