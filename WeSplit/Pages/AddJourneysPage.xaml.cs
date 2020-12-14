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

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for ManageJourneysPage.xaml
	/// </summary>
	public partial class AddJourneysPage : Page
	{
		public AddJourneysPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void addRouteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void viewLargeMapButton_Click(object sender, RoutedEventArgs e)
		{
			//call dialog
		}

		private void deleteRelativeImageInListButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addMemberButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void addExpensesButton_Click(object sender, RoutedEventArgs e)
		{

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
	}
}
