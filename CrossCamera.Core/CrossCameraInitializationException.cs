using System;

namespace CrossCamera.Core
{
	internal class CrossCameraInitializationException : Exception
	{
		public CrossCameraInitializationException() : base("CrossCamera not initialized.")
		{
		}
	}
}
