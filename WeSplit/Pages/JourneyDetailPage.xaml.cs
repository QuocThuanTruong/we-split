using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		Journey _journey;
		public JourneyDetailPage()
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);
		}

		public JourneyDetailPage(int ID_Journey)
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);

			_ID_Journey = ID_Journey;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_journey = _databaseUtilities.GetJourneyByID(_ID_Journey);

			var expenseSeriesCollection = new SeriesCollection();
			foreach (var expense in _journey.Expenses)
            {
				expenseSeriesCollection.Add(new PieSeries
				{
					Title = expense.Expenses_Description,
					Values = new ChartValues<int> { decimal.ToInt32(expense.Expenses_Money ?? 0) }
				});
			}

			expensesChart.Series = expenseSeriesCollection;

			var receivablesSeriesCollection = new SeriesCollection();
			foreach (var member in _journey.JourneyAttendances)
			{
				receivablesSeriesCollection.Add(new PieSeries
				{
					Title = member.Member_Name,
					Values = new ChartValues<int> { decimal.ToInt32(member.Receivables_Money ?? 0) }
				});
			}

			receivablesChart.Series = receivablesSeriesCollection;

			this.DataContext = _journey;
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
			Route startRoute = new Route();
			startRoute.Place = _journey.Start_Place;
			startRoute.Province = _journey.Start_Province;

			Site endSite = _databaseUtilities.GetSiteByID(_journey.ID_Site.Value);
			Province endProvince = _databaseUtilities.GetProvinceByID(endSite.ID_Province);

			Route endRoute = new Route();
			endRoute.Place = _journey.Site_Name;
			endRoute.Province = endProvince.Province_Name;

			visualRouteDetailDialog.ShowDialog(_journey.Route_For_Binding.ToList(), startRoute, endRoute);
		}

		private void updateJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateJourney?.Invoke(_ID_Journey);
		}

		private void finishJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			_databaseUtilities.FinishJourney(_ID_Journey);
		}


		private void map_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void visualRouteDetailDialog_CloseFullScreenVideoDialog()
		{

		}

	}
}
