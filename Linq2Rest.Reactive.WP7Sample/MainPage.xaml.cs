// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.Reactive.WP7Sample
{
	using System;
	using System.Reactive.Linq;
	using Microsoft.Phone.Controls;

	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			Loaded += MainPage_Loaded;
			InitializeComponent();
		}

		void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			var observable = new RestObservable<SampleDto>(new FakeAsyncRestClientFactory(), new PhoneSerializerFactory());
			var subscription = observable
				.Where(x => x.Text != "blah")
				.Subscribe(
						   x => txtStatus.Text = "Got one",
						   ex => txtStatus.Text = "Uh oh",
						   () => Dispatcher.BeginInvoke(() => txtStatus.Text = "Finished"));
		}
	}
}