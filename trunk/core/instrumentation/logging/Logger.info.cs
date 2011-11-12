using System;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        public void Info(string message) {
            internalLogger.Info(message);
        }

        public void Info<T>(T value) {
            internalLogger.Info(value);
        }

        public void Info(string message, object[] args) {
            internalLogger.Info(String.Format(message, args));
        }

        public void Info<TArgument>(string message, TArgument argument) {
            internalLogger.Info(String.Format(message, argument));
        }

        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2) {
            internalLogger.Info(String.Format(message, argument1, argument2));
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                             TArgument3 argument3) {
            internalLogger.Info(String.Format(message, argument1, argument2, argument3));
        }

        //public void Info(IFormatProvider formatProvider, string message, params object[] args) {
        //    internalLogger.Info(formatProvider, message, args);
        //}

        //public void Info<T>(IFormatProvider formatProvider, T value) {
        //    internalLogger.Info(formatProvider, value);
        //}

        //public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) {
        //    internalLogger.Info(formatProvider, message, argument);
        //}

        //public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
        //                                         TArgument2 argument2) {
        //    internalLogger.Info(formatProvider, message, argument1, argument2);
        //}

        //public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
        //                                                     TArgument1 argument1, TArgument2 argument2,
        //                                                     TArgument3 argument3) {
        //    internalLogger.Info(formatProvider, message, argument1, argument2, argument3);
        //}

        public void Info(string message, Exception exception) {
            internalLogger.Info(message, exception);
        }
    }
}