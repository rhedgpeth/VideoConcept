﻿using Xamarin.Forms;
using VideoConcept.Core.ViewModels;

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
	}
}