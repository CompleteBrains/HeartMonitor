using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Application.Compositions;
using Application.Extensions;
using Caliburn.Micro;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Config;
using NLog.Targets;
using ViewModels;


namespace Application
{
	public class Bootstrapper : BootstrapperBase
	{
		private IUnityContainer container;

	    public Bootstrapper()
	    {
	        Initialize();
	    }

	    protected override void Configure()
		{
			container = new UnityContainer();
			container.AddNewExtension<Composition>();

	        ConfigureCaliburn();
	        ConfigureNLog();

	        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		}

	    private void ConfigureCaliburn()
		{
			ViewLocator.NameTransformer.AddRule("Model", string.Empty);
			AssemblySource.Instance.Add(Assembly.GetAssembly(typeof (Views.Shell)));

			container.RegisterType<IWindowManager, WindowManager>(new Singleton());
			container.RegisterType<IEventAggregator, EventAggregator>(new Singleton());
		}

	    private void ConfigureNLog()
		{
			var config = new LoggingConfiguration();

			var target = new FileTarget();
			config.AddTarget("file", target);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));

			target.FileName = "${basedir}/Recorder.log";
			target.Layout = @"${longdate} | ${level:padding=-5} | ${logger:padding=-22} | ${message} ";

			NLog.LogManager.Configuration = config;

			container.AddNewExtension<BuildTracking>();
			container.AddNewExtension<LogCreation>();
		}

	    protected override object GetInstance(Type service, string key)
		{
			var instance = container.Resolve(service, key);
			if (instance != null)
				return instance;

			throw new InvalidOperationException("Could not locate any instances.");
		}

	    protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.ResolveAll(service);
		}

	    protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}

	    protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<IShell>();
		}

	    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs arguments)
	    {
            var handler = container.Resolve<ErrorsHandler>();
	        handler.OnUnhandledException((Exception) arguments.ExceptionObject);

            OnExit(sender, arguments);
        }

        protected override void OnExit(object sender, EventArgs e)
		{
			container.Resolve<ILogger>().Info("End\n");

			base.OnExit(sender, e);
		}
	}
}