using System;

namespace munimji.core.instrumentation.logging {
#if false
    public partial class Logger
    {
        #region Trace

        public void Trace(string message)
        {
            internalLogger.Trace(message);
        }

        public void Trace<T>(T value)
        {
            internalLogger.Trace(value);
        }

        public void Trace(string message, object[] args)
        {
            internalLogger.Trace(message, args);
        }

        public void Trace<TArgument>(string message, TArgument argument)
        {
            internalLogger.Trace(message, argument);
        }

        public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            internalLogger.Trace(message, argument1, argument2);
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            internalLogger.Trace(message, argument1, argument2, argument3);
        }

        public void Trace(IFormatProvider formatProvider, string message, params object[] args)
        {
            internalLogger.Trace(formatProvider, message, args);
        }

        public void Trace<T>(IFormatProvider formatProvider, T value)
        {
            internalLogger.Trace(formatProvider, value);
        }

        public void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            internalLogger.Trace(formatProvider, message, argument);
        }

        public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1,
                                                  TArgument2 argument2)
        {
            internalLogger.Trace(formatProvider, message, argument1, argument2);
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message,
                                                              TArgument1 argument1, TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            internalLogger.Trace(formatProvider, message, argument1, argument2, argument3);
        }

        public void Trace(string message, Exception exception)
        {
            internalLogger.Trace(message, exception);
        }

        #endregion
    } 
#endif
}