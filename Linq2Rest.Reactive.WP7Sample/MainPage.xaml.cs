using Microsoft.Phone.Controls;

namespace Linq2Rest.Reactive.WP7Sample
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;

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
						   () =>
						   {
							   try
							   {
								   Dispatcher.BeginInvoke(() => txtStatus.Text = "Finished");
							   }
							   catch (Exception ex)
							   {

							   }
						   });
		}
	}
}