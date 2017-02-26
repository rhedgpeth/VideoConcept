using System;
using VideoConcept.Core.Services;
using VideoConcept.Core.Utilities;

namespace VideoConcept.Shared
{
	public static class Bootstrap
	{
		public static void Init()
		{
			ServiceContainer.Register<IFileService>(() => new FileService());
		}
	}
}
