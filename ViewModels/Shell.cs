using Caliburn.Micro;
using ViewModels.Services;

namespace ViewModels
{
	public interface IShell {}

    public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
		public Shell()
		{
		}

        [Notify]
	    public string Received { get; set; }
	    
	    public void Send()
	    {
	        
	    }
	}
}