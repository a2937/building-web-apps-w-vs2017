using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SpyStore.DAL.Logging
{
    public static class LogHelper
    {
        public static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    }
}
