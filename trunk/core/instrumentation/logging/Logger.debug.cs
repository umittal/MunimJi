using System;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        #region Debug

        public void Debug(string message) {
            internalLogger.Debug(message);
        }

        public void Debug<T>(T value) {
            internalLogger.Debug(value);
        }

        public void Debug(string message, object[] args) {
            internalLogger.Debug(String.Format(message,args));
        }

        public void Debug<TArgument>(string message, TArgument argument) {
            internalLogger.Debug(String.Format(message, argument));
        }

        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2) {
            internalLogger.Debug(String.Format(message, argument1, argument2));
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                              TArgument3 argument3) {
            internalLogger.Debug(String.Format(message, argument1, argument2, argument3));
        }

        public void Debug(IFormatProvider formatProvider, string message, params object[] args) {
            internalLogger.Debug(String.Format(formatProvider, message, args));
        }

        //public void Debug<T>(IFormatProvider formatProvider, T value) {
        //    internalLogger.Debug();
        //}

        //public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) {
        //    internalLogger.Debug(formatProvider, message, argument);
        //}

        //public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
        //                                          TArgument2 argument2) {
        //    internalLogger.Debug(formatProvider, message, argument1, argument2);
        //}

        //public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
        //                                                      TArgument1 argument1, TArgument2 argument2,
        //                                                      TArgument3 argument3) {
        //    internalLogger.Debug(formatProvider, message, argument1, argument2, argument3);
        //}

        public void Debug(string message, Exception exception) {
            internalLogger.Debug(message, exception);
        }

        #endregion
    }
}