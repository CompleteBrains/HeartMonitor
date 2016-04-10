using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Threading;
using Caliburn.Micro;
using Common;
using MoreLinq;
using NLog;

namespace ViewModels.Panel
{
	public interface IShell {}

	public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
		private readonly IButtonsHandler buttons;
		private readonly ISettings settings;
		private Stopwatch stopwatch;
		private DispatcherTimer timer;
		private readonly Logger log;

		public Shell(IScreen[] viewModels, ISettings settings, IButtonsHandler buttons, IFolderSelector folder)
		{
			this.settings = settings;
			this.buttons = buttons;

			Items.AddRange(viewModels);

			log = NLog.LogManager.GetLogger("Duration.Shell");
			CreateTimer();

		    folder.Changed += () => NotifyOfPropertyChange(nameof(Output));
		}

	    public bool CanStart { get; private set; } = true;
	    public bool CanStop { get; private set; }
	    public string Output => settings.OutputPath;

	    public string Timer { get; set; }

	    private void CreateTimer()
	    {
	        stopwatch = new Stopwatch();

	        timer = new DispatcherTimer(DispatcherPriority.Render);
	        timer.Interval = TimeSpan.FromMilliseconds(10);

	        timer.Tick += (s, a) =>
	        {
	            Timer = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.f");
	            NotifyOfPropertyChange(nameof(Timer));
	        };
	    }

	    public void Start()
		{
            if (!Directory.Exists(Output))
                Directory.CreateDirectory(Path.GetDirectoryName(Output));

            settings.RecordsCounter++;

			stopwatch.Restart();
			timer.Start();

			buttons.Start();

			CanStart = false;
			CanStop = true;
			NotifyAllProperties();
		}

		public void Stop()
		{
			log.Trace(String.Empty);
			log.Trace(stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff") + " - Stopwatch");
			buttons.Stop();
			log.Trace(stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff") + " - Stopwatch");
			log.Trace(String.Empty);

			stopwatch.Stop();
            timer.Stop();

			CanStart = true;
			CanStop = false;
			NotifyAllProperties();
		}

		public void SelectFolder() => buttons.SelectFolder();

		private void NotifyAllProperties()
		{
			NotifyOfPropertyChange(nameof(CanStart));
			NotifyOfPropertyChange(nameof(CanStop));
		}
	}
}