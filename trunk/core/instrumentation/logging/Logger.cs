using System.Reflection;
using log4net;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        //private static readonly Logger instance;
        private readonly ILog internalLogger;

        //static Logger() {
        //    instance = new Logger();
        //}

        private Logger(ILog logger) {
            internalLogger = logger;
        }

        public static Logger GetInstance(string name) {
            
            return new Logger(LogManager.GetLogger(name));
        }

        public static Logger GetInstance()
        {
            return new Logger(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name));
        }


        //public static Logger Instance {
        //    get { return instance; }
        //}
    }
}