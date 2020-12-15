using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeSplit.Utilities;

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for JourneyDetailPage.xaml
	/// </summary>
	public partial class JourneyDetailPage : Page
	{
		public delegate void UpdateJourneyHandler(int journeyID);
		public event UpdateJourneyHandler UpdateJourney;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();
		private int _ID_Journey;
		public JourneyDetailPage()
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);
		}

		public JourneyDetailPage(int ID_Journey)
		{
			InitializeComponent();

			_ID_Journey = ID_Journey;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Journey journey = _databaseUtilities.GetJourneyByID(_ID_Journey);

			this.DataContext = journey;
		}

		private void currentJourneyProgess_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (currentJourneyProgess.Value == 0)
			{
				startIcon.Visibility = Visibility.Hidden;
			}
			else if (currentJourneyProgess.Value == 100)
			{
				endIcon.Visibility = Visibility.Hidden;
			}
			else
			{
				startIcon.Visibility = Visibility.Visible;
				if (endIcon != null)
				{
					endIcon.Visibility = Visibility.Visible;
				}
			}
		}

		private void viewLargeMapButton_Click(object sender, RoutedEventArgs e)
		{
			visualRouteDetailDialog.ShowDialog();
		}

		private void updateJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateJourney?.Invoke(0);
		}

		private void finishJourneyButton_Click(object sender, RoutedEventArgs e)
		{

		}


		private void map_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void visualRouteDetailDialog_CloseFullScreenVideoDialog()
		{

		}

	}
}
