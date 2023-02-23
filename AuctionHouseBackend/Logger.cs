using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend
{
    public enum LogLevel
    {
        INFO,
        DEBUG,
        WARNING,
        ERROR
    }
    public class Logger
    {
        public static void AddLog(LogLevel level, string message)
        {
            string msg = $"{DateTime.Now} ";
            string logLevel = "";
            switch (level)
            {
                case LogLevel.INFO:
                    logLevel = "INFO";
                    break;
                case LogLevel.DEBUG:
                    logLevel = "DEBUG";
                    break;
                case LogLevel.WARNING:
                    logLevel = "WARNING";
                    break;
                case LogLevel.ERROR:
                    logLevel = "ERROR";
                    break;
                default:
                    break;
            }
            msg += logLevel + " " + message;

            File.WriteAllText("Logger.txt", msg);
        }
    }
}
