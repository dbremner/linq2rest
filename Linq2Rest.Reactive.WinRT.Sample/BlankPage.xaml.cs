using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Linq2Rest.Reactive.Implementations;
using Linq2Rest.Reactive.WinRT.Sample.Models;
using Linq2Rest.Reactive.WinRT.Sample.Support;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Linq2Rest.Reactive.WinRT.Sample
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class BlankPage : Page
	{
		public BlankPage()
		{
			InitializeComponent();
			Resources.Add("FilmList", new ObservableCollection<NetflixFilm>());
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var filmList = Resources["FilmList"] as ObservableCollection<NetflixFilm>;
			films.ItemsSource = filmList;
			var observable =
				new RestObservable<NetflixFilm>(
					new AsyncJsonRestClientFactory(
						new Uri("http://odata.netflix.com/v2/Catalog/Titles")),
					new ODataSerializerFactory());

			observable
				.Where(x => x.Name.Contains("harry") && x.BoxArt.MediumUrl != null)
				.Subscribe(x => Dispatcher.InvokeAsync(
					CoreDispatcherPriority.Normal,
					AddFilm,
					this,
					x));
		}

		private void AddFilm(object sender, InvokedHandlerArgs e)
		{
			var filmList = Resources["FilmList"] as ObservableCollection<NetflixFilm>;
			var film = e.Context as NetflixFilm;
			filmList.Add(film);
		}
	}
}
