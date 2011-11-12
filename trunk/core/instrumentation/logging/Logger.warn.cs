using System;

namespace munimji.core.instrumentation.logging {
    public partial class Logger {
        public void Warn(string message)
        {
            internalLogger.Warn(message);
        }

        public void Warn<T>(T value)
        {
            internalLogger.Warn(value);
        }

        public void Warn(string message, object[] args)
        {
            internalLogger.Warn(String.Format(message, args));
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            internalLogger.Warn(String.Format(message, argument));
        }

        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            internalLogger.Warn(String.Format(message, argument1, argument2));
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                             TArgument3 argument3)
        {
            internalLogger.Warn(string.Format(message, argument1, argument2, argument3));
        }

        //public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        //{
        //    internalLogger.Warn(formatProvider, message, args);
        //}

        //public void Warn<T>(IFormatProvider formatProvider, T value)
        //{
        //    internalLogger.Warn(formatProvider, value);
        //}

        //public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        //{
        //    internalLogger.Warn(formatProvider, message, argument);
        //}

        //public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
        //                                         TArgument2 argument2)
        //{
        //    internalLogger.Warn(formatProvider, message, argument1, argument2);
        //}

        //public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
        //                                                     TArgument1 argument1, TArgument2 argument2,
        //                                                     TArgument3 argument3)
        //{
        //    internalLogger.Warn(formatProvider, message, argument1, argument2, argument3);
        //}

        public void Warn(string message, Exception exception)
        {
            internalLogger.Warn(message, exception);
        }
    }
}