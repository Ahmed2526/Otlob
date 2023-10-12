using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface ILoggerManager
    {
        public void LogDebug(string message);
        public void LogError(string message);
        public void LogInfo(string message);
        public void LogWarn(string message);
    }
}
