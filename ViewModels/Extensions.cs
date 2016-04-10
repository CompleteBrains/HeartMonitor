using Caliburn.Micro;

namespace ViewModels
{
	public static class Extensions
	{
		public static void Publish(this IEventAggregator eventAggregator, object message)
		{
			eventAggregator.PublishOnUIThread(message);
		}

	}
}