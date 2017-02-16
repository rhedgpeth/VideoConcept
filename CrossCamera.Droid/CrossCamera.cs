using System;
using System.IO;

namespace CrossCamera.Droid
{
	public static class CrossCamera
	{
		public static void Initialize()
		{
			Core.Camera.Current = new DroidCamera();
			Core.CrossCamera.isInitialized = true;
		}
	}
}
