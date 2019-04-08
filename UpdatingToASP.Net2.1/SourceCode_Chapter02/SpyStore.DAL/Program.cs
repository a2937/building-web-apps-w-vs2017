using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;


namespace SpyStore.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            //LogHelper._logger.Info("Logger Initialized");
        }
    }
}
