using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mqd.SqlHelper
{
    /// <summary>
    /// 数据库命令执行异常
    /// </summary>
    internal class DbCmdException : Exception
    {
        private string _message;

        public DbCmdException(string message)
        {
            _message = message;
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        public override string Message { get { return _message; } }
    }
}
