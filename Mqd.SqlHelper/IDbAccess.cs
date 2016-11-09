using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 定义一组公共数据访问接口
    /// </summary>
    internal interface IDbAccess
    {
        /// <summary>
        /// 获取指定表所有记录
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns></returns>
        DataTable GetAll(string table);
    }
}
