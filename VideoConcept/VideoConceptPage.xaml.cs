using Xamarin.Forms;
using CrossCamera.Core;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace VideoConcept
{
	public partial class VideoConceptPage : ContentPage
	{
		public VideoConceptPage()
		{
			InitializeComponent();

			var weakThis = new WeakReference<ContentPage>(this);
			Func<string, string, string, Task> displayAlert = (title, message, cancel) =>
			{
				ContentPage conceptPage = null;
				if (weakThis.TryGetTarget(out conceptPage))
				{
					return conceptPage.DisplayAlert(title, message, cancel);
				}
				return Task.Run(() => { });
			};

			BindingContext = new VideoConceptViewModel(Camera.Current, displayAlert);
		}

		bool _isInitialized;

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (!_isInitialized)
			{
				await (BindingContext as VideoConceptViewModel).Initialize();
				_isInitialized = true;
			}
		}
	}
}
