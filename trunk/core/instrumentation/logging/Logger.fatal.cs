using System;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        public void Fatal(string message) {
            internalLogger.Fatal(message);
        }

        public void Fatal<T>(T value) {
            internalLogger.Fatal(value);
        }

        public void Fatal(string message, object[] args) {
            internalLogger.Fatal(String.Format(message, args));
        }

        public void Fatal<TArgument>(string message, TArgument argument) {
            internalLogger.Fatal(String.Format(message, argument));
        }

        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2) {
            internalLogger.Fatal(String.Format(message, argument1, argument2));
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                              TArgument3 argument3) {
            internalLogger.Fatal(String.Format(message, argument1, argument2, argument3));
        }

        //public void Fatal(IFormatProvider formatProvider, string message, params object[] args) {
        //    internalLogger.Fatal(formatProvider, message, args);
        //}

        //public void Fatal<T>(IFormatProvider formatProvider, T value) {
        //    internalLogger.Fatal(formatProvider, value);
        //}

        //public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) {
        //    internalLogger.Fatal(formatProvider, message, argument);
        //}

        //public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
        //                                          TArgument2 argument2) {
        //    internalLogger.Fatal(formatProvider, message, argument1, argument2);
        //}

        //public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
        //                                                      TArgument1 argument1, TArgument2 argument2,
        //                                                      TArgument3 argument3) {
        //    internalLogger.Fatal(formatProvider, message, argument1, argument2, argument3);
        //}

        public void Fatal(string message, Exception exception) {
            internalLogger.Fatal(message, exception);
        }
    }
}