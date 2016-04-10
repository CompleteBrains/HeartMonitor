using Caliburn.Micro;

namespace ViewModels
{
	public interface IShell {}

	public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
		public Shell(IScreen[] viewModels)
		{
		    Items.AddRange(viewModels);
		}
	}
}