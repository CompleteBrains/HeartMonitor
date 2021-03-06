﻿using Microsoft.Practices.Unity;

namespace Application.Compositions
{
	public class Composition : UnityContainerExtension
	{
		protected override void Initialize()
		{
            Container.RegisterType<ViewModels.IShell, ViewModels.Shell>();
            Container.RegisterType<Views.IDialog, Views.Dialog>();
		}
	}
}