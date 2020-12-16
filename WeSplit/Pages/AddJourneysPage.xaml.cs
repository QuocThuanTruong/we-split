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
		private int _ordinal_number = 0;
		public AddJourneysPage()
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);
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
			GetMemberByIDJourney_Result member = new GetMemberByIDJourney_Result();
			JourneyAttendance journeyAttendance = new JourneyAttendance();

			journeyAttendance.ID_Journey = _journey.ID_Journey;

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

			journeyAttendance.Receivables_Money = decimal.Parse(memberReceiptMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
			member.Receivables_Money = journeyAttendance.Receivables_Money;


			string[] roles = { "Trưởng nhóm", "Thành viên" };
			journeyAttendance.Role = roles[memberRoleComboBox.SelectedIndex];
			member.Role = journeyAttendance.Role;

			//Reset
			memberNameTextBox.Text = "";
			memberPhoneTextBox.Text = "";
			memberReceiptMoneyTextBox.Text = "";
			memberRoleComboBox.SelectedIndex = 0;

			_journey.Members_For_Binding.Add(member);
			_journey.JourneyAttendances.Add(journeyAttendance);

			membersListView.ItemsSource = _journey.Members_For_Binding.ToList();
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
