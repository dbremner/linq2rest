// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.WP7Sample
{
	using System;
	using System.Collections.ObjectModel;
	using System.Reactive.Linq;
	using Linq2Rest.Reactive.WP7Sample.Models;
	using Linq2Rest.Reactive.WP7Sample.Support;
	using Microsoft.Phone.Controls;

	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			Loaded += MainPage_Loaded;
			Resources.Add("Films", new ObservableCollection<NetflixFilm>());
			InitializeComponent();
		}

		void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			var films = Resources["Films"] as ObservableCollection<NetflixFilm>;
			// http://odata.netflix.com/v2/Catalog/Titles
			var observable = new RestObservable<NetflixFilm>(
				new AsyncJsonRestClientFactory(new Uri("http://odata.netflix.com/v2/Catalog/Titles")),
				new ODataSerializerFactory());
			var subscription = observable
				.Where(x => x.Name.Contains("harry"))
				.Subscribe(
						   x => Dispatcher.BeginInvoke(() => films.Add(x)),
						   ex => Dispatcher.BeginInvoke(() => txtStatus.Text = ex.Message),
						   () => Dispatcher.BeginInvoke(() => txtStatus.Text = "Finished"));
		}
	}
}