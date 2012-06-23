// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.WP7Sample
{
	using System;
	using System.Collections.ObjectModel;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using Linq2Rest.Reactive.WP7Sample.Models;
	using Linq2Rest.Reactive.WP7Sample.Support;
	using Microsoft.Phone.Controls;

	public partial class MainPage : PhoneApplicationPage
	{
		private readonly RestObservable<NugetPackage> _nugetObservable;
		private readonly ObservableCollection<NugetPackage> _packageCollection;
		// Constructor
		public MainPage()
		{
			_packageCollection = new ObservableCollection<NugetPackage>();
			Resources.Add("Packages", _packageCollection);
			InitializeComponent();
			_nugetObservable = new RestObservable<NugetPackage>(
				new AsyncJsonRestClientFactory(new Uri("http://nuget.org/api/v2/Packages")),
				new ODataSerializerFactory());
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			_packageCollection.Clear();
			var subscription = _nugetObservable
				.Create()
				.Where(x => x.Dependencies.Contains(txtSearch.Text) && x.IsLatestVersion)
				.Subscribe(
						   x => Dispatcher.BeginInvoke(() => _packageCollection.Add(x)),
						   ex => Dispatcher.BeginInvoke(() => txtStatus.Text = ex.Message),
						   () => Dispatcher.BeginInvoke(() => txtStatus.Text = "Finished"));
		}
	}
}