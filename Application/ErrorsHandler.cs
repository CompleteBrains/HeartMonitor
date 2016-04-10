using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Unity;
using NLog;

namespace Application
{
    public class ErrorsHandler
    {
        private readonly ILogger log;

        public ErrorsHandler(ILogger log)
        {
            this.log = log;
        }

        public void OnUnhandledException(Exception exception)
        {
            exception = GetException(exception);

            if (!Debugger.IsAttached)
                MessageBox.Show(exception.Message, exception.TargetSite.ReflectedType?.FullName, MessageBoxButton.OK,
                                MessageBoxImage.Error);

            log.Fatal(exception);

        }

        private Exception GetException(Exception unhandled)
        {
            Exception exception;

            if (unhandled is ResolutionFailedException)
                exception = unhandled.InnerException;
            else if (unhandled is TargetInvocationException)
                exception = unhandled.InnerException.InnerException;
            else
                exception = unhandled;

            return exception;
        }
    }
}