using System;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        public void Error(string message) {
            internalLogger.Error(message);
        }

        public void Error<T>(T value) {
            internalLogger.Error(value);
        }

        public void Error(string message, object[] args) {
            internalLogger.Error(String.Format(message, args));
        }

        public void Error<TArgument>(string message, TArgument argument) {
            internalLogger.Error(String.Format(message, argument));
        }

        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2) {
            internalLogger.Error(string.Format(message, argument1, argument2));
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                              TArgument3 argument3) {
            internalLogger.Error(String.Format(message, argument1, argument2, argument3));
        }

        //public void Error(IFormatProvider formatProvider, string message, params object[] args) {
        //    internalLogger.Error(formatProvider, message, args);
        //}

        //public void Error<T>(IFormatProvider formatProvider, T value) {
        //    internalLogger.Error(formatProvider, value);
        //}

        //public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) {
        //    internalLogger.Error(formatProvider, message, argument);
        //}

        //public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
        //                                          TArgument2 argument2) {
        //    internalLogger.Error(formatProvider, message, argument1, argument2);
        //}

        //public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
        //                                                      TArgument1 argument1, TArgument2 argument2,
        //                                                      TArgument3 argument3) {
        //    internalLogger.Error(formatProvider, message, argument1, argument2, argument3);
        //}

        public void Error(string message, Exception exception) {
            internalLogger.Error(message, exception);
        }
    }
}