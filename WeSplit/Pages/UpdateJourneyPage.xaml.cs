using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
		private List<Province> _provinces;
		private int _ID_Journey;
		Journey _journey;

		public List<Expens> Expens_For_Binding;
		public List<Advance> Advances_For_Binding;
		public List<Route> Route_For_Binding;
		public List<JourneyAttendance> JourneyAttendances;
		public List<JourneyImage> Images_For_Binding;

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

			//Get journey for update
            _journey = _databaseUtilities.GetJourneyByID(_ID_Journey);

			//Detach List for binding from journey
			Route_For_Binding = new List<Route>(_journey.Route_For_Binding);
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

			Province province = _databaseUtilities.GetProvinceByName(_journey.Start_Province);
			startProvinceComboBox.SelectedIndex = province.ID_Province - 1;

			Site site = _databaseUtilities.GetSiteByID(_journey.ID_Site ?? 0);
			endProvinceComboBox.SelectedIndex = site.ID_Province;
			endSiteComboBox.SelectedIndex = _journey.ID_Site - 1 ?? 0;

			if (Images_For_Binding.Count > 0)
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
			Route route = new Route();

			route.ID_Journey = _journey.ID_Journey;
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

			if (routesListView.SelectedIndex != -1)
            {
				route.Route_Index = Route_For_Binding[routesListView.SelectedIndex].Route_Index;
				route.Ordinal_Number = route.Route_Index;

				Route_For_Binding[routesListView.SelectedIndex] = route;
				_journey.Route_For_Binding[routesListView.SelectedIndex] = route;

				routesListView.SelectedIndex = -1;
			} 
			else
            {
				route.Route_Index = Route_For_Binding.Count;
				route.Ordinal_Number = route.Route_Index;
				route.Is_Active = 1;

				Route_For_Binding.Add(route);
				_journey.Route_For_Binding.Add(route);
			}

			routesListView.ItemsSource = null;
			routesListView.ItemsSource = Route_For_Binding;
		}

		private void addAdvanceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void updateAdvanceButton_Click(object sender, RoutedEventArgs e)
		{
			Advance advance = (Advance)advanceListView.SelectedItem;


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

			Images_For_Binding.RemoveAt(int.Parse(clickedButton.Tag.ToString()));

			updateRelativeImageIndex();
		}

		private void updateRelativeImageIndex()
		{
			var index = 0;

			foreach (var image in Images_For_Binding)
			{
				image.ImageIndex = index++;
			}

			if (Images_For_Binding.Count == 0)
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
						image.ImageIndex = imageIdx++;

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
			_journey.Route_For_Binding[Route_Index].Is_Active = 0;

			Route_For_Binding.RemoveAt(Route_Index);

			routesListView.ItemsSource = null;
			routesListView.ItemsSource = Route_For_Binding;
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

        private void haveDoneButton_Click(object sender, RoutedEventArgs e)
        {
			int Route_Index = int.Parse(((ToggleButton)sender).Tag.ToString());

			bool isDoneRoute = ((ToggleButton)sender).IsChecked.Value;

			Route_For_Binding[Route_Index].Route_Status = isDoneRoute ? 1 : 0;

			_journey.Route_For_Binding[Route_Index].Route_Status = isDoneRoute ? 1 : 0;
		}
    }
}
