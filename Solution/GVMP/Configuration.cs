#define BUILD_RELEASE
//#define BUILD_DEBUG
using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class Configuration
    {
        public static readonly bool whitelist = false;
        
#if BUILD_RELEASE
        private static readonly string host = "localhost";
        private static readonly string username = "root";
        private static readonly string database = "nexuscrimelife";

        #region SENSIBEL
        private static readonly string password = "";
        #endregion
#endif

#if BUILD_DEBUG
        private static readonly string host = "localhost";
        private static readonly string username = "root";
        private static readonly string database = "gvmp";

        #region SENSIBEL
        private static readonly string password = "";
        #endregion
#endif

        public static string connectionString = "Server=" + Configuration.host + "; Database=" + Configuration.database + "; UID=" + Configuration.username + "; PASSWORD=" + Configuration.password;
    }
}
