using System;
using System.IO;

namespace CrossCamera.iOS
{
	public static class CrossCamera
	{
		public static void Initialize()
		{
			Core.Camera.Current = new iOSCamera();
			Core.CrossCamera.isInitialized = true;
		}
	}
}
