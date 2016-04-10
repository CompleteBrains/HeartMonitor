using Microsoft.Practices.Unity;
using ViewModels;

namespace Application.Compositions
{
	public class Composition : UnityContainerExtension
	{
		protected override void Initialize()
		{
            Container.RegisterType<IShell, Shell>();
		}
	}
}