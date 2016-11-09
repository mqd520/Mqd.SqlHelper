using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 数据库连接异常
    /// </summary>
    internal class DbConnException : Exception
    {
        private string _message;

        public DbConnException(string message)
        {
            _message = message;
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        public override string Message { get { return _message; } }
    }
}
