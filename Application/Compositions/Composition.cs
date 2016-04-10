using Microsoft.Practices.Unity;
using PerResolve = Microsoft.Practices.Unity.PerResolveLifetimeManager;
using Constructor = Microsoft.Practices.Unity.InjectionConstructor;
using Method = Microsoft.Practices.Unity.InjectionMethod;
using Singleton = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;


namespace Application.Compositions
{
	public class Composition : UnityContainerExtension
	{
		protected override void Initialize()
		{
		}
	}
}