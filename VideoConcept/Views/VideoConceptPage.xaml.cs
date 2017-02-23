using Xamarin.Forms;
using VideoConcept.Core.ViewModels;
using VideoConcept.Messages;

namespace VideoConcept
{
	public partial class VideoConceptPage : ContentPage
	{
		VideoConceptViewModel ViewModel { get; set; }

		public VideoConceptPage()
		{
			InitializeComponent();
			BindingContext = ViewModel = new VideoConceptViewModel(DisplayAlert);
		}

		bool _isInitialized;

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (!_isInitialized)
			{
				await ViewModel.Initialize();
				_isInitialized = true;
			}
		}

		public void Refresh()
		{
			ViewModel.Refresh();
		}
	}
}
