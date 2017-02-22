using Xamarin.Forms;
using VideoConcept.Core;

namespace VideoConcept
{
	public partial class App : Application
	{
		public App(string environmentalDocumentPath)
		{
			InitializeComponent();

			Global.EnvironmentalDocumentsPath = environmentalDocumentPath;

			MainPage = new VideoConceptPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
