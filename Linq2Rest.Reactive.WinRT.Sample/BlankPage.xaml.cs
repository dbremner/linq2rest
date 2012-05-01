using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Linq2Rest.Reactive.Implementations;
using Linq2Rest.Reactive.WinRT.Sample.Models;
using Linq2Rest.Reactive.WinRT.Sample.Support;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void AddFilm(object sender, InvokedHandlerArgs e)
		{
			var film = e.Context as NetflixFilm;
			films.Items.Add(film);
		}

		private void OnSearch(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var button = (sender as Button);
			button.IsEnabled = false;
			if (films.Items.Any())
			{
				films.Items.Clear();
			}

			var query = search.Text;
			new RestObservable<NetflixFilm>(
				new AsyncJsonRestClientFactory(
					new Uri("http://odata.netflix.com/v2/Catalog/Titles")),
				new ODataSerializerFactory())
				.Create()
				.Where(x => x.Name.Contains(query))
				.Subscribe(
					x => Dispatcher.InvokeAsync(CoreDispatcherPriority.Normal, AddFilm, this, x),
					() => Dispatcher.InvokeAsync(CoreDispatcherPriority.Normal, (s, ea) => button.IsEnabled = true, this, null));
		}
	}
}
