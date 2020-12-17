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
	/// Interaction logic for UpdateJourneyPage.xaml
	/// </summary>
	public partial class UpdateJourneyPage : Page
	{
		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();
		private List<Province> _provinces;
		private int _ID_Journey;
		Journey _journey;

		public UpdateJourneyPage(int ID_Journey)
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);

			_ID_Journey = ID_Journey;
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            startProvinceRouteComboBox.ItemsSource = _provinces;
            startProvinceComboBox.ItemsSource = _provinces;
            endProvinceComboBox.ItemsSource = _provinces;

            _journey = _databaseUtilities.GetJourneyByID(_ID_Journey);

			_provinces = _databaseUtilities.GetListProvince();
			startProvinceRouteComboBox.ItemsSource = _provinces;
            startProvinceComboBox.ItemsSource = _provinces;
            endProvinceComboBox.ItemsSource = _provinces;

			Province province = _databaseUtilities.GetProvinceByName(_journey.Start_Province);
			startProvinceComboBox.SelectedIndex = province.ID_Province - 1;

			Site site = _databaseUtilities.GetSiteByID(_journey.ID_Site ?? 0);
			endProvinceComboBox.SelectedIndex = site.ID_Province;
			endSiteComboBox.SelectedIndex = _journey.ID_Site - 1 ?? 0;

			if (_journey.Images_For_Binding.Count > 0)
			{
				addImageOption1Button.Visibility = Visibility.Collapsed;
				addImageOption2Button.Visibility = Visibility.Visible;
				journeyImageListView.Visibility = Visibility.Visible;
			}


			
			DataContext = _journey;
        }

		private void addMemberButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addExpensesButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addRouteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addAdvanceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void updateAdvanceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void updateExpensesButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void updateMemberButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void updateRouteButton_Click(object sender, RoutedEventArgs e)
		{

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

		private void deleteRelativeImageInListButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void cancelAddRecipeButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void saveJourneyButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addImageButton_Click(object sender, RoutedEventArgs e)
		{
			//nếu list image có hình thì ẩn nút option 1 đi :v
			addImageOption1Button.Visibility = Visibility.Collapsed;
			addImageOption2Button.Visibility = Visibility.Visible;
			journeyImageListView.Visibility = Visibility.Visible;
		}

		private void deleteRouteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void deleteMemberButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void deleteExpensesButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void deleteAdvancesButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void visualRouteDetailDialog_CloseFullScreenVideoDialog()
		{
			
		}

		private void map_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

        private void endProvinceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			Province province = (Province)endProvinceComboBox.SelectedItem;

			List<Site> sites = _databaseUtilities.GetListSiteByProvince(province.ID_Province);

			endSiteComboBox.ItemsSource = sites;
			endSiteComboBox.SelectedIndex = 0;
		}
    }
}
