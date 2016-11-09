using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 数据库分类
    /// </summary>
    internal enum EDbCatagory
    {
        /// <summary>
        /// SqlServer数据库
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// Oracl数据库
        /// </summary>
        Oracl = 2,

        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql = 3,

        /// <summary>
        /// Access数据库
        /// </summary>
        Access = 4
    }
}
