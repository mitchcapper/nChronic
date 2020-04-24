using System;
using System.Diagnostics;

namespace Chronic
{
    public static class Logger
    {
        public static Action<string> LogCmd = (msg) => Debug.WriteLine(msg);
        public static void Log(Func<string> message)
        {
            if (Parser.IsDebugMode)
            {
                LogCmd(message());
            }

        }
    }
}