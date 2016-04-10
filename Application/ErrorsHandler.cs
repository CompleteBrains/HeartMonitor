using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Unity;
using NLog;
using Views;

namespace Application
{
    public class ErrorsHandler
    {
        private readonly ILogger log;
        private readonly IDialog dialog;

        public ErrorsHandler(ILogger log, IDialog dialog)
        {
            this.log = log;
            this.dialog = dialog;
        }


        public void OnUnhandledException(Exception exception)
        {
            exception = GetException(exception);

            if (!Debugger.IsAttached)
                dialog.Show(exception);

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