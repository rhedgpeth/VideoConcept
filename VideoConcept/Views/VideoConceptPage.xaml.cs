using Xamarin.Forms;
using VideoConcept.Core.ViewModels;
using System.Threading.Tasks;

namespace VideoConcept
{
	public partial class VideoConceptPage : ContentPage
	{
		VideoConceptViewModel ViewModel { get; set; }

		public VideoConceptPage()
		{
			InitializeComponent();

			var vm = new VideoConceptViewModel(DisplayAlert);

			BindingContext = vm;

			// Load the list of videos with any outstanding in the table
			vm.Initialize();
		}
	}
}
