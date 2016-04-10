using Microsoft.Practices.Unity;
using ViewModels;
using Views;
using Shell = ViewModels.Shell;

namespace Application.Compositions
{
	public class Composition : UnityContainerExtension
	{
		protected override void Initialize()
		{
            Container.RegisterType<IShell, Shell>();
            Container.RegisterType<IDialog, Dialog>();
		}
	}
}