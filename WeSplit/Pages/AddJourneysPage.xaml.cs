using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Interaction logic for ManageJourneysPage.xaml
	/// </summary>
	public partial class AddJourneysPage : Page
	{
		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();
		private GoogleMapUtilities _googleMapUtilities = GoogleMapUtilities.GetGoogleMapInstance();
		private BingMapUtilities _bingMapUtilities = BingMapUtilities.GetBingMapInstance();
		private AppUtilities _appUtilities = AppUtilities.GetAppInstance();

		private Journey _journey = new Journey();
		private List<Province> _provinces;

		private int _ordinal_number = 0;
		private int _maxIDMember = 0;
		private int _maxIDExpenses = 0;
		public AddJourneysPage()
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);

			_maxIDMember = _databaseUtilities.GetMaxIDMember() + 1;
			_maxIDExpenses = _databaseUtilities.GetMaxIDExpenses() + 1;
			_provinces = _databaseUtilities.GetListProvince();

			startProvinceRouteComboBox.ItemsSource = _provinces;
			startProvinceComboBox.ItemsSource = _provinces;
			endProvinceComboBox.ItemsSource = _provinces;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_journey.ID_Journey = _databaseUtilities.GetMaxIDJourney() + 1;

			ImageCard.Visibility = Visibility.Collapsed;
			AdvanceCard.Visibility = Visibility.Collapsed;

		}

		private void addRouteButton_Click(object sender, RoutedEventArgs e)
		{
			Route route = new Route();

			route.ID_Journey = _journey.ID_Journey;
			route.Ordinal_Number = ++_ordinal_number;
			route.Route_Status = 0;

			route.Place = routeStartPlaceTextBox.Text;
			if (route.Place.Length <= 0)
            {
				
				return;
            }

			route.Route_Description = descriptionRouteTextBox.Text;
			if (route.Route_Description.Length <= 0)
			{

				return;
			}

			route.Province = ((Province)startProvinceRouteComboBox.SelectedItem).Province_Name;

			routeStartPlaceTextBox.Text = "";
			descriptionRouteTextBox.Text = "";

			_journey.Routes.Add(route);

			routesListView.ItemsSource = _journey.Routes.ToList();
		}

		private void viewLargeMapButton_Click(object sender, RoutedEventArgs e)
		{
			Route startRoute = new Route();
			startRoute.Place = journeyStartPlaceTextBox.Text;
			startRoute.Province = ((Province)startProvinceComboBox.SelectedItem).Province_Name;

			Route endRoute = new Route();
			if (endSiteComboBox.SelectedItem == null)
            {
				endRoute.Place = "";
			} 
			else
            {
				endRoute.Place = ((Site)endSiteComboBox.SelectedItem).Site_Name;

			}
			endRoute.Province = ((Province)endProvinceComboBox.SelectedItem).Province_Name;

			visualRouteDetailDialog.ShowDialog(_journey.Routes.ToList(), startRoute, endRoute);
		}

		private void visualRouteDetailDialog_CloseFullScreenVideoDialog()
		{

		}

		private void deleteRelativeImageInListButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addMemberButton_Click(object sender, RoutedEventArgs e)
		{
			JourneyAttendance member = new JourneyAttendance();
			member.ID_Member = _maxIDMember++;
			member.ID_Journey = _journey.ID_Journey;
			member.Member_Index = _journey.JourneyAttendances.Count + 1;

			member.Member_Name = memberNameTextBox.Text;
			if (member.Member_Name.Length <= 0)
			{

				return;
			}

			member.Phone_Number = memberPhoneTextBox.Text;
			if (member.Phone_Number.Length <= 0)
			{

				return;
			}

			member.Receivables_Money = decimal.Parse(memberReceiptMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
			member.Money_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(member.Receivables_Money ?? 0));

			string[] roles = { "Trưởng nhóm", "Thành viên" };
			member.Role = roles[memberRoleComboBox.SelectedIndex];

			//Reset
			memberNameTextBox.Text = "";
			memberPhoneTextBox.Text = "";
			memberReceiptMoneyTextBox.Text = "";
			memberRoleComboBox.SelectedIndex = 0;

			_journey.JourneyAttendances.Add(member);

			membersListView.ItemsSource = _journey.JourneyAttendances.ToList();
		}

		private void addExpensesButton_Click(object sender, RoutedEventArgs e)
		{
			Expens expens = new Expens();

			expens.ID_Journey = _journey.ID_Journey;
			expens.ID_Expenses = _maxIDExpenses++;

			expens.Expenses_Money = decimal.Parse(expensesMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
			expens.Expenses_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(expens.Expenses_Money ?? 0));

			expens.Expenses_Description = descriptionExpensesTextBox.Text;
			if (expens.Expenses_Description.Length <= 0)
			{

				return;
			}

			expensesMoneyTextBox.Text = "";
			descriptionExpensesTextBox.Text = "";

			_journey.Expenses.Add(expens);

			expensesListView.ItemsSource = _journey.Expenses.ToList();
		}

		private void addAdvanceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void saveJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			//Get Data
			_journey.Journey_Name = journeyNameTextBox.Text;
			if (_journey.Journey_Name.Length == 0)
            {
				return;
            }

			_journey.Start_Place = journeyStartPlaceTextBox.Text;
			if (_journey.Start_Place.Length == 0)
            {
				return;
            }

			_journey.Start_Province = ((Province)startProvinceComboBox.SelectedItem).Province_Name;

			if (isCurrentJourneyCheckBox.IsChecked.Value)
            {
				_journey.Status = 0;

				_databaseUtilities.FinishCurrentJourney();
			} 
			else
            {
				_journey.Status = 1;
			}

			Route startRoute = new Route();
			startRoute.Place = _journey.Start_Place;
			startRoute.Province = _journey.Start_Province;

			Route endRoute = new Route();
			endRoute.Place = ((Site)endSiteComboBox.SelectedItem).Site_Name;
			endRoute.Province = ((Province)endProvinceComboBox.SelectedItem).Province_Name;
			_journey.ID_Site = ((Site)endSiteComboBox.SelectedItem).ID_Site;

			_journey.StartDate = startDatePicker.SelectedDate;
			_journey.EndDate = endDatePicker.SelectedDate;

			//Get distance
			_journey.Distance = _googleMapUtilities.GetDistanceByRoutes(_journey.Routes.ToList(), startRoute, endRoute);

			//Insert 
			_databaseUtilities.AddNewJourney(
				_journey.ID_Journey,
				_journey.Journey_Name,
				_journey.ID_Site,
				_journey.Start_Place,
				_journey.Start_Province,
				_journey.Status,
				_journey.StartDate,
				_journey.EndDate,
				_journey.Distance);

			foreach(var expense in _journey.Expenses)
            {
				_databaseUtilities.AddExpense(expense.ID_Expenses, expense.ID_Journey, expense.Expenses_Money, expense.Expenses_Description);
            }

			foreach (var route in _journey.Routes)
			{
				_databaseUtilities.AddRoute(route.ID_Journey, route.Ordinal_Number, route.Place, route.Province, route.Route_Description, route.Route_Status);
			}

			foreach (var member in _journey.JourneyAttendances)
            {
				_databaseUtilities.AddJourneyAttendance(member.ID_Member, member.ID_Journey, member.Member_Name, member.Phone_Number, member.Receivables_Money, member.Role);
            }

			_journey = new Journey();
			_journey.ID_Journey = _databaseUtilities.GetMaxIDJourney() + 1;

			notiMessageSnackbar.MessageQueue.Enqueue($"Đã thêm thành công chuyến đi \"{_journey.Journey_Name}\"", "OK", () => { });

			//Reset
			journeyNameTextBox.Text = "";
			journeyStartPlaceTextBox.Text = "";
			startProvinceComboBox.SelectedIndex = 0;
			startDatePicker.Text = "";
			endSiteComboBox.SelectedIndex = 0;
			endProvinceComboBox.SelectedIndex = 0;
			endDatePicker.Text = "";
			startProvinceRouteComboBox.SelectedIndex = 0;
			routesListView.ItemsSource = null;
			membersListView.ItemsSource = null;
			expensesListView.ItemsSource = null;
			_ordinal_number = 0;
		}

		private void cancelAddRecipeButton_Click(object sender, RoutedEventArgs e)
		{
			//Reset
			journeyNameTextBox.Text = "";
			journeyStartPlaceTextBox.Text = "";
			startProvinceComboBox.SelectedIndex = 0;
			startDatePicker.Text = "";
			endSiteComboBox.SelectedIndex = 0;
			endProvinceComboBox.SelectedIndex = 0;
			endDatePicker.Text = "";
			startProvinceRouteComboBox.SelectedIndex = 0;
			routesListView.ItemsSource = null;
			membersListView.ItemsSource = null;
			expensesListView.ItemsSource = null;
			_ordinal_number = 0;
		}

		private void addImageButton_Click(object sender, RoutedEventArgs e)
		{
			//nếu list image có hình thì ẩn nút option 1 đi :v
			addImageOption1Button.Visibility = Visibility.Collapsed;
			addImageOption2Button.Visibility = Visibility.Visible;
			journeyImageListView.Visibility = Visibility.Visible;
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
