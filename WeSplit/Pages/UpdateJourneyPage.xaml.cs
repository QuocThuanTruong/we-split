using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
		private AppUtilities _appUtilities = AppUtilities.GetAppInstance();
		private GoogleMapUtilities _googleMapUtilities = GoogleMapUtilities.GetGoogleMapInstance();
		private List<Province> _provinces;
		private int _ID_Journey;
		Journey _journey;

		public List<Expens> Expens_For_Binding;
		public List<Advance> Advances_For_Binding;
		public List<Route> Route_For_Binding;
		public List<JourneyAttendance> JourneyAttendances;
		public List<JourneyImage> Images_For_Binding;

		private int _ordinal_number = 0;
		private int _ordinal_number_image = 0;
		private int _max_id_member = 0;
		public UpdateJourneyPage(int ID_Journey)
		{
			InitializeComponent();
			visualRouteDetailDialog.SetParent(mainContainer);

			_ID_Journey = ID_Journey;
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
			//Get journey for update
            _journey = _databaseUtilities.GetJourneyByID(_ID_Journey);

			//Detach List for binding from journey
			Route_For_Binding = new List<Route>(_journey.Route_For_Binding);
			for (int i = 0; i < Route_For_Binding.Count; ++i)
            {
				Route_For_Binding[i].Standard_Place = _appUtilities.getStandardName(Route_For_Binding[i].Place, 20);
				Route_For_Binding[i].Standard_Description = _appUtilities.getStandardName(Route_For_Binding[i].Route_Description, 20);
			}
			
			JourneyAttendances = new List<JourneyAttendance>(_journey.JourneyAttendances.ToList());
			Images_For_Binding = new List<JourneyImage>(_journey.Images_For_Binding);
			Expens_For_Binding = new List<Expens>(_journey.Expens_For_Binding);
			Advances_For_Binding = new List<Advance>(_journey.Advances_For_Binding);

			//Set ItemSource
			routesListView.ItemsSource = Route_For_Binding;
			membersListView.ItemsSource = JourneyAttendances;
			journeyImageListView.ItemsSource = Images_For_Binding;
			expensesListView.ItemsSource = Expens_For_Binding;
			advanceListView.ItemsSource = Advances_For_Binding;

			//For combo box
			_provinces = _databaseUtilities.GetListProvince();
			startProvinceRouteComboBox.ItemsSource = _provinces;
            startProvinceComboBox.ItemsSource = _provinces;
            endProvinceComboBox.ItemsSource = _provinces;
			borrowerComboBox.ItemsSource = JourneyAttendances;
			lenderComboBox.ItemsSource = JourneyAttendances;

			Province province = _databaseUtilities.GetProvinceByName(_journey.Start_Province);
			startProvinceComboBox.SelectedIndex = province.ID_Province - 1;

			Site site = _databaseUtilities.GetSiteByID(_journey.ID_Site ?? 0);
			endProvinceComboBox.SelectedIndex = site.ID_Province - 1;

			if (Images_For_Binding.Count > 0)
			{
				addImageOption1Button.Visibility = Visibility.Collapsed;
				addImageOption2Button.Visibility = Visibility.Visible;
				journeyImageListView.Visibility = Visibility.Visible;
			}

			_ordinal_number = _databaseUtilities.GetMaxOrdinalNumber(_ID_Journey);
			_ordinal_number_image = _databaseUtilities.GetMaxOrdinalNumberImage(_ID_Journey);
			_max_id_member = _databaseUtilities.GetMaxIDMember();

			DataContext = _journey;
        }

		private void addMemberButton_Click(object sender, RoutedEventArgs e)
		{
			JourneyAttendance member = new JourneyAttendance();

			member.ID_Journey = _journey.ID_Journey;
			member.Is_Active = 1;
			//member.Member_Index = _journey.JourneyAttendances.Count + 1;

			member.Member_Name = memberNameTextBox.Text;
			if (member.Member_Name.Length <= 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tên thành viên", "OK", () => { });
				return;
			}

			member.Phone_Number = memberPhoneTextBox.Text;
			if (member.Phone_Number.Length <= 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống SĐT của thành viên", "OK", () => { });
				return;
			}

			if (memberReceiptMoneyTextBox.Text.Length == 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tiền thu của thành viên", "OK", () => { });
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

			if (membersListView.SelectedIndex != -1)
			{
				member.Member_Index = JourneyAttendances[membersListView.SelectedIndex].Member_Index;
				member.ID_Member = JourneyAttendances[membersListView.SelectedIndex].ID_Member;

				JourneyAttendances[membersListView.SelectedIndex] = member;

				membersListView.SelectedIndex = -1;
			}
			else
			{
				member.Member_Index = JourneyAttendances.Count + 1;
				member.ID_Member = ++_max_id_member;

				member.Is_Active = 1;

				JourneyAttendances.Add(member);
			}

			membersListView.ItemsSource = null;
			membersListView.ItemsSource = JourneyAttendances;

			borrowerComboBox.ItemsSource = null;
			borrowerComboBox.ItemsSource = JourneyAttendances;

			lenderComboBox.ItemsSource = null;
			lenderComboBox.ItemsSource = JourneyAttendances;
		}

		private void addExpensesButton_Click(object sender, RoutedEventArgs e)
		{
			Expens expens = new Expens();

			expens.ID_Journey = _journey.ID_Journey;
			expens.Is_Active = 1;

			if (expensesMoneyTextBox.Text.Length == 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tiền thu", "OK", () => { });
				return;
			}

			expens.Expenses_Money = decimal.Parse(expensesMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
			expens.Expenses_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(expens.Expenses_Money ?? 0));

			expens.Expenses_Description = descriptionExpensesTextBox.Text;
			if (expens.Expenses_Description.Length <= 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tiền mô tả khoản chi", "OK", () => { });
				return;
			}

			expensesMoneyTextBox.Text = "";
			descriptionExpensesTextBox.Text = "";

			if (expensesListView.SelectedIndex != -1)
			{
				expens.ID_Expenses = Expens_For_Binding[expensesListView.SelectedIndex].ID_Expenses;
				expens.Expense_Index = Expens_For_Binding[expensesListView.SelectedIndex].Expense_Index;

				Expens_For_Binding[expensesListView.SelectedIndex] = expens;

				expensesListView.SelectedIndex = -1;
			}
			else
			{
				expens.ID_Expenses = _databaseUtilities.GetMaxIDExpenses() + 1;
				expens.Expense_Index = Expens_For_Binding.Count + 1;

				expens.Is_Active = 1;

				Expens_For_Binding.Add(expens);
			}

			expensesListView.ItemsSource = null;
			expensesListView.ItemsSource = Expens_For_Binding;
		}

		private void addRouteButton_Click(object sender, RoutedEventArgs e)
		{
			Route route = new Route();

			route.ID_Journey = _journey.ID_Journey;
			route.Route_Status = 0;
			route.Is_Active = 1;

			route.Place = routeStartPlaceTextBox.Text;
			if (route.Place.Length <= 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống địa điểm", "OK", () => { });
				return;
			}
			route.Standard_Place = _appUtilities.getStandardName(route.Place, 20);

			route.Route_Description = descriptionRouteTextBox.Text;
			if (route.Route_Description.Length <= 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống phần mô tả", "OK", () => { });
				return;
			}
			route.Standard_Description = _appUtilities.getStandardName(route.Route_Description, 20);

			route.Province = ((Province)startProvinceRouteComboBox.SelectedItem).Province_Name;

			routeStartPlaceTextBox.Text = "";
			descriptionRouteTextBox.Text = "";

			if (routesListView.SelectedIndex != -1)
            {
				route.Route_Index = Route_For_Binding[routesListView.SelectedIndex].Route_Index;
				route.Ordinal_Number = Route_For_Binding[routesListView.SelectedIndex].Ordinal_Number;

				Route_For_Binding[routesListView.SelectedIndex] = route;

				routesListView.SelectedIndex = -1;
			} 
			else
            {
				route.Route_Index = Route_For_Binding.Count + 1;
				route.Ordinal_Number = ++_ordinal_number;
				route.Is_Active = 1;

				Route_For_Binding.Add(route);
			}

			routesListView.ItemsSource = null;
			routesListView.ItemsSource = Route_For_Binding;
		}

		private void addAdvanceButton_Click(object sender, RoutedEventArgs e)
		{
			Advance advance = new Advance();
			advance.ID_Journey = _journey.ID_Journey;
			advance.Is_Active = 1;

			if (borrowerComboBox.SelectedIndex == -1)
            {
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống người mượn", "OK", () => { });
				return;
            } 

			advance.ID_Borrower = JourneyAttendances[borrowerComboBox.SelectedIndex].ID_Member;
			advance.Borrower_Name = _databaseUtilities.GetMemberNameByID(advance.ID_Borrower);

			if (lenderComboBox.SelectedIndex == -1)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống người cho mượn", "OK", () => { });
				return;
			}

			advance.ID_Lender = JourneyAttendances[lenderComboBox.SelectedIndex].ID_Member;
			advance.Lender_Name = _databaseUtilities.GetMemberNameByID(advance.ID_Lender);

			if (advanceMoneyTextBox.Text.Length == 0)
            {
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống số tiền mượn", "OK", () => { });
			}

			advance.Advance_Money = decimal.Parse(advanceMoneyTextBox.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
			advance.Money_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(advance.Advance_Money ?? 0));

			if (advanceListView.SelectedIndex != -1)
			{
				advance.Advance_Index = Advances_For_Binding[advanceListView.SelectedIndex].Advance_Index;

				Advances_For_Binding[advanceListView.SelectedIndex] = advance;

				advanceListView.SelectedIndex = -1;
			}
			else
			{
				advance.Advance_Index = Advances_For_Binding.Count + 1;

				advance.Is_Active = 1;

				Advances_For_Binding.Add(advance);
			}

			borrowerComboBox.SelectedIndex = -1;
			lenderComboBox.SelectedIndex = -1;
			advanceMoneyTextBox.Text = "";

			advanceListView.ItemsSource = null;
			advanceListView.ItemsSource = Advances_For_Binding;
		}

		private void updateAdvanceButton_Click(object sender, RoutedEventArgs e)
		{
			Advance advance = (Advance)advanceListView.SelectedItem;

			for (int i = 0; i < JourneyAttendances.Count; ++i)
            {
				if (advance.Borrower_Name == JourneyAttendances[i].Member_Name)
                {
					borrowerComboBox.SelectedIndex = i;
					break;
                }
            }

			for (int i = 0; i < JourneyAttendances.Count; ++i)
			{
				if (advance.Lender_Name == JourneyAttendances[i].Member_Name)
				{
					lenderComboBox.SelectedIndex = i;
					break;
				}
			}

			advanceMoneyTextBox.Text = decimal.ToInt32(advance.Advance_Money ?? 0).ToString();
		}

		private void updateExpensesButton_Click(object sender, RoutedEventArgs e)
		{
			Expens expens = (Expens)expensesListView.SelectedItem;

			descriptionExpensesTextBox.Text = expens.Expenses_Description;
			expensesMoneyTextBox.Text = decimal.ToInt32(expens.Expenses_Money ?? 0).ToString();
		}

		private void updateMemberButton_Click(object sender, RoutedEventArgs e)
		{
			JourneyAttendance member = (JourneyAttendance)membersListView.SelectedItem;

			memberNameTextBox.Text = member.Member_Name;
			memberPhoneTextBox.Text = member.Phone_Number;
			memberReceiptMoneyTextBox.Text = decimal.ToInt32(member.Receivables_Money ?? 0).ToString();

			if (member.Role == "Trưởng nhóm") {
				memberRoleComboBox.SelectedIndex = 0;
			} else {
				memberRoleComboBox.SelectedIndex = 1;
            }

		}

		private void updateRouteButton_Click(object sender, RoutedEventArgs e)
		{
			Route route = (Route)routesListView.SelectedItem;

			routeStartPlaceTextBox.Text = route.Place;
			descriptionRouteTextBox.Text = route.Route_Description;

			Province province = _databaseUtilities.GetProvinceByName(route.Province);
			startProvinceRouteComboBox.SelectedIndex = province.ID_Province;
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
			var clickedButton = (System.Windows.Controls.Button)sender;

			Debug.WriteLine(clickedButton.Tag);

			Images_For_Binding[int.Parse(clickedButton.Tag.ToString()) - 1].Is_Active = 0;

			updateRelativeImageIndex();
		}

		private void updateRelativeImageIndex()
		{
			var index = 0;
			var total_collapsed_image = 0;

			foreach (var image in Images_For_Binding)
			{
				if (image.Is_Active == 0)
                {
					++total_collapsed_image;

				}
			}

			if (Images_For_Binding.Count == total_collapsed_image)
			{
				journeyImageListView.Visibility = Visibility.Collapsed;
				addImageOption1Button.Visibility = Visibility.Visible;
				addImageOption2Button.Visibility = Visibility.Collapsed;
			}
			else
			{
				journeyImageListView.ItemsSource = null;
				journeyImageListView.ItemsSource = Images_For_Binding;
			}
		}

		private void cancelAddRecipeButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void saveJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			//Get Data
			_journey.Journey_Name = journeyNameTextBox.Text;
			if (_journey.Journey_Name.Length == 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tên chuyến đi", "OK", () => { });
				return;
			}

			_journey.Start_Place = journeyStartPlaceTextBox.Text;
			if (_journey.Start_Place.Length == 0)
			{
				notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống địa điểm xuất phát", "OK", () => { });
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
				
			}

			Route startRoute = new Route();
			startRoute.Place = _journey.Start_Place;
			startRoute.Province = _journey.Start_Province;

			if (endSiteComboBox.SelectedIndex == -1)
            {
				notiMessageSnackbar.MessageQueue.Enqueue($"Hiện tại chưa có địa điểm nào được thêm ở tỉnh {((Province)endProvinceComboBox.SelectedItem).Province_Name}", "OK", () => { });
				notiMessageSnackbar.MessageQueue.Enqueue($"Vui lòng chọn địa điểm khác hoặc thêm địa điểm mới vào tỉnh {((Province)endProvinceComboBox.SelectedItem).Province_Name} và thử lại", "OK", () => { });
				return;
			}

			Route endRoute = new Route();
			endRoute.Place = ((Site)endSiteComboBox.SelectedItem).Site_Name;
			endRoute.Province = ((Province)endProvinceComboBox.SelectedItem).Province_Name;
			_journey.ID_Site = ((Site)endSiteComboBox.SelectedItem).ID_Site;

			_journey.StartDate = startDatePicker.SelectedDate;
			_journey.EndDate = endDatePicker.SelectedDate;

			//Get distance
			var routeForCalcDistance = (from r in Route_For_Binding
										where r.Is_Active == 1
										select r).ToList();

			_journey.Distance = _googleMapUtilities.GetDistanceByRoutes(routeForCalcDistance, startRoute, endRoute);

			//Insert 
			_databaseUtilities.UpdateJourney(
				_journey.ID_Journey,
				_journey.Journey_Name,
				_journey.ID_Site,
				_journey.Start_Place,
				_journey.Start_Province,
				_journey.Status,
				_journey.StartDate,
				_journey.EndDate,
				_journey.Distance);

            foreach (var expense in Expens_For_Binding)
            {
                _databaseUtilities.UpdateExpense(expense.ID_Expenses, expense.ID_Journey, expense.Expenses_Money, expense.Expenses_Description, expense.Is_Active ?? 0);
            }

            foreach (var route in Route_For_Binding)
            {
                _databaseUtilities.UpdateRoute(route.ID_Journey, route.Ordinal_Number, route.Place, route.Province, route.Route_Description, route.Route_Status, route.Is_Active ?? 0);
            }

            foreach (var member in JourneyAttendances)
            {
                _databaseUtilities.UpdateJourneyAttendance(member.ID_Member, member.ID_Journey, member.Member_Name, member.Phone_Number, member.Receivables_Money, member.Role, member.Is_Active ?? 0);
            }

			_appUtilities.createIDDirectory(_journey.ID_Journey);
			foreach (var image in Images_For_Binding)
            {
				_databaseUtilities.UpdateJourneyImage(_journey.ID_Journey, image.Ordinal_Number, _appUtilities.getTypeOfImage(image.Link_Image), image.Is_Active);

				_appUtilities.copyImageToIDirectory(_journey.ID_Journey, image.Link_Image, $"{image.Ordinal_Number}", false);
            }

			foreach (var advance in Advances_For_Binding)
			{
				_databaseUtilities.UpdateAdvance(advance.ID_Journey, advance.ID_Borrower, advance.ID_Lender, advance.Advance_Money, advance.Is_Active);
			}

			notiMessageSnackbar.MessageQueue.Enqueue($"Đã update thành công chuyến đi \"{_journey.Journey_Name}\"", "OK", () => { });

			//Reset
			//journeyNameTextBox.Text = "";
			//journeyStartPlaceTextBox.Text = "";
			//startProvinceComboBox.SelectedIndex = 0;
			//startDatePicker.Text = "";
			//endSiteComboBox.SelectedIndex = 0;
			//endProvinceComboBox.SelectedIndex = 0;
			//endDatePicker.Text = "";
			//startProvinceRouteComboBox.SelectedIndex = 0;
			//routesListView.ItemsSource = null;
			//membersListView.ItemsSource = null;
			//expensesListView.ItemsSource = null;
		}

		private void addImageButton_Click(object sender, RoutedEventArgs e)
		{
			//nếu list image có hình thì ẩn nút option 1 đi :v
			//addImageOption1Button.Visibility = Visibility.Collapsed;
			//addImageOption2Button.Visibility = Visibility.Visible;
			//journeyImageListView.Visibility = Visibility.Visible;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Multiselect = true;
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.ico)|*.png;*.jpeg;*.jpg;*.ico";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					var imageIdx = 0;

					foreach (var fileName in openFileDialog.FileNames)
					{
						JourneyImage image = new JourneyImage();

						image.Link_Image = fileName;
						image.ImageIndex = Images_For_Binding.Count + 1;
						image.Is_Active = 1;
						image.Ordinal_Number = ++_ordinal_number_image;

						Images_For_Binding.Add(image);
					}

					addImageOption1Button.Visibility = Visibility.Collapsed;
					addImageOption2Button.Visibility = Visibility.Visible;
					journeyImageListView.Visibility = Visibility.Visible;
					journeyImageListView.ItemsSource = null;
					journeyImageListView.ItemsSource = Images_For_Binding;
				}
			}
		}

		private void deleteRouteButton_Click(object sender, RoutedEventArgs e)
		{
			int Route_Index = int.Parse(((System.Windows.Controls.Button)sender).Tag.ToString());
			Route_For_Binding[Route_Index - 1].Is_Active = 0;

			routesListView.ItemsSource = null;
			routesListView.ItemsSource = Route_For_Binding;
		}

		private void deleteMemberButton_Click(object sender, RoutedEventArgs e)
		{
			int Member_Index = int.Parse(((System.Windows.Controls.Button)sender).Tag.ToString());
			JourneyAttendances[Member_Index - 1].Is_Active = 0;

			membersListView.ItemsSource = null;
			membersListView.ItemsSource = JourneyAttendances;

			borrowerComboBox.ItemsSource = null;
			borrowerComboBox.ItemsSource = JourneyAttendances;

			lenderComboBox.ItemsSource = null;
			lenderComboBox.ItemsSource = JourneyAttendances;
		}

		private void deleteExpensesButton_Click(object sender, RoutedEventArgs e)
		{
			int Expense_Index = int.Parse(((System.Windows.Controls.Button)sender).Tag.ToString());
			Expens_For_Binding[Expense_Index - 1].Is_Active = 0;

			expensesListView.ItemsSource = null;
			expensesListView.ItemsSource = Expens_For_Binding;
		}

		private void deleteAdvancesButton_Click(object sender, RoutedEventArgs e)
		{
			int Advance_Index = int.Parse(((System.Windows.Controls.Button)sender).Tag.ToString());
			Advances_For_Binding[Advance_Index - 1].Is_Active = 0;

			advanceListView.ItemsSource = null;
			advanceListView.ItemsSource = Advances_For_Binding;
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

			Site site = _databaseUtilities.GetSiteByID(_journey.ID_Site ?? 0);

			if (site.ID_Province == province.ID_Province)
            {
				for (int i = 0; i < sites.Count; ++i)
					if (sites[i].Site_Name == site.Site_Name)
                    {
						endSiteComboBox.SelectedIndex = i;
					}
            } else
            {
				endSiteComboBox.SelectedIndex = 0;
			}

			
		}

        private void haveDoneButton_Click(object sender, RoutedEventArgs e)
        {
			int Route_Index = int.Parse(((ToggleButton)sender).Tag.ToString());

			bool isDoneRoute = ((ToggleButton)sender).IsChecked.Value;

			Route_For_Binding[Route_Index].Route_Status = isDoneRoute ? 1 : 0;

			_journey.Route_For_Binding[Route_Index].Route_Status = isDoneRoute ? 1 : 0;
		}

        private void memberPhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}

        private void memberReceiptMoneyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}

        private void expensesMoneyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}

        private void advanceMoneyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}
    }
}
