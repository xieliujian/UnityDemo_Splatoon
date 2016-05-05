using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splatoon
{
    public enum LogLevel
    {
        Error,
        Warn,
        Info,
        Debug
    }

    public class LogSystem
    {
        public static void Write(LogLevel level, string format, params object[] args)
        {

        }
    }
}
