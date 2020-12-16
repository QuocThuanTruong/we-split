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
		private Journey _journey = new Journey();
		private List<Province> _provinces;
		private int _ordinal_number = 0;
		private int _maxIDMember = 0;
		public AddJourneysPage()
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);

			_maxIDMember = _databaseUtilities.GetMaxIDMember() + 1;
			_provinces = _databaseUtilities.GetListProvince();

			startProvinceComboBox.ItemsSource = _provinces;
			endProvinceComboBox.ItemsSource = _provinces;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_journey.ID_Journey = _databaseUtilities.GetMaxIDJourney();
		}

		private void addRouteButton_Click(object sender, RoutedEventArgs e)
		{
			Route route = new Route();

			route.ID_Journey = _journey.ID_Journey;
			route.Ordinal_Number = ++_ordinal_number;

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

			routeStartPlaceTextBox.Text = "";
			descriptionRouteTextBox.Text = "";

			_journey.Routes.Add(route);

			routesListView.ItemsSource = _journey.Routes.ToList();

		}

		private void viewLargeMapButton_Click(object sender, RoutedEventArgs e)
		{
			visualRouteDetailDialog.ShowDialog();
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
			member.Receivables_Money = member.Receivables_Money;


			string[] roles = { "Trưởng nhóm", "Thành viên" };
			member.Role = roles[memberRoleComboBox.SelectedIndex];
			member.Role = member.Role;

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
			expens.ID_Expenses = _journey.Expenses.Count + 1;

			expens.Expenses_Money = decimal.Parse(expensesMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));

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
			_journey.Status = 1;

			_journey.StartDate = startDatePicker.SelectedDate;
			_journey.EndDate = endDatePicker.SelectedDate;

			//Insert 
			 
		}

		private void cancelAddRecipeButton_Click(object sender, RoutedEventArgs e)
		{

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

	
	}
}
