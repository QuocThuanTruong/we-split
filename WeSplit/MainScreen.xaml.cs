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
using System.Windows.Shapes;
using WeSplit.Pages;

namespace WeSplit
{
	/// <summary>
	/// Interaction logic for MainScreen.xaml
	/// </summary>
	public partial class MainScreen : Window
	{

		private List<Tuple<Button, Image, string, string, TextBlock>> _mainScreenButtons;

		public MainScreen()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			HomePage homePage = new HomePage();
			homePage.ShowJourneyDetailPage += MainScreen_ShowJourneyDetailPage;
			pageNavigation.NavigationService.Navigate(homePage);

			_mainScreenButtons = new List<Tuple<Button, Image, string, string, TextBlock>>()
			{
				new Tuple<Button, Image, string, string, TextBlock>(homePageButton, iconHomePage, "IconGreenHome", "IconWhiteHome", homePageName),
				new Tuple<Button, Image, string, string, TextBlock>(mngJourneyPageButton, iconMngJourneyPage, "IconGreenLuggage", "IconWhiteLuggage", mngJourneyPageName),
				new Tuple<Button, Image, string, string, TextBlock>(addSitePageButton, iconAddSitePage, "IconGreenMarker", "IconWhiteMarker", addSitePageName),
				new Tuple<Button, Image, string, string, TextBlock>(helpPageButton, iconHelpPage, "IconGreenHelp", "IconWhiteHelp", helpPageName),
				new Tuple<Button, Image, string, string, TextBlock>(aboutPageButton, iconAboutPage, "IconGreenAbout", "IconWhiteAbout", aboutPageName)
			};

			//Default load page is home page
			DrawerButton_Click(homePageButton, e);
		}

		private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void closeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void minimizeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void DrawerButton_Click(object sender, RoutedEventArgs e)
		{
			/** Highlight selected button**/
			var selectedButton = (Button)sender;

			/** Default property of button
			 * <Setter Property="Background" Value="Transparent"/>
			 * <Setter Property="BorderThickness" Value="1"/>**/

			foreach (var button in _mainScreenButtons)
			{
				if (button.Item1.Name != selectedButton.Name)
				{
					button.Item1.Background = Brushes.Transparent;
					button.Item1.IsEnabled = true;

					button.Item2.Source = (ImageSource)FindResource(button.Item3);
					button.Item5.Foreground = (Brush)FindResource("MyDarkGreen");
				}
			}

			//Highlight
			selectedButton.Background = new LinearGradientBrush((Color)FindResource("StartGradientColor"), (Color)FindResource("EndGradientColor"), 0.0);
			selectedButton.IsEnabled = false;
			/****/

			/** Navigating page **/
			pageNavigation.NavigationService.Navigate(getPageFromButton(selectedButton));
		}

		private Page getPageFromButton(Button selectedButton)
		{
			Page result = new HomePage();

			if (selectedButton.Name == homePageButton.Name)
			{
				iconHomePage.Source = (ImageSource)FindResource(_mainScreenButtons[0].Item4);
				homePageName.Foreground = Brushes.White;
				result = new HomePage();
				((HomePage)result).ViewAllJourney += MainScreen_ViewAllJourney;
				((HomePage)result).ShowJourneyDetailPage += MainScreen_ShowJourneyDetailPage;
			}
			else if (selectedButton.Name == mngJourneyPageButton.Name)
			{
				iconMngJourneyPage.Source = (ImageSource)FindResource(_mainScreenButtons[1].Item4);
				mngJourneyPageName.Foreground = Brushes.White;
				result = new AddJourneysPage();
				 
			}
			else if (selectedButton.Name == addSitePageButton.Name)
			{
				iconAddSitePage.Source = (ImageSource)FindResource(_mainScreenButtons[2].Item4);
				addSitePageName.Foreground = Brushes.White;
				result = new AddSitePage();
				//((AddRecipePage)result).BackToHome += MainScreen_BackToHome;
			}
			else if (selectedButton.Name == helpPageButton.Name)
			{
				iconHelpPage.Source = (ImageSource)FindResource(_mainScreenButtons[3].Item4);
				helpPageName.Foreground = Brushes.White;
				result = new HelpPage();
			}
			else if (selectedButton.Name == aboutPageButton.Name)
			{
				iconAboutPage.Source = (ImageSource)FindResource(_mainScreenButtons[4].Item4);
				aboutPageName.Foreground = Brushes.White;
				result = new AboutPage();
			}

			return result;
		}

		private void MainScreen_ShowJourneyDetailPage(int ID_Journey)
		{
			JourneyListPage_ShowJourneyDetailPage(ID_Journey);
		}

		private void MainScreen_ViewAllJourney(int journeyStatus)
		{
			JourneyListPage journeyListPage = new JourneyListPage(journeyStatus);
			journeyListPage.ShowJourneyDetailPage += JourneyListPage_ShowJourneyDetailPage;
			pageNavigation.NavigationService.Navigate(journeyListPage);
		}

		private void JourneyListPage_ShowJourneyDetailPage(int ID_Journey)
		{
			JourneyDetailPage journeyDetailPage = new JourneyDetailPage(ID_Journey);
			journeyDetailPage.UpdateJourney += JourneyDetailPage_UpdateJourney;

			pageNavigation.NavigationService.Navigate(journeyDetailPage);


			//clear selected button
			foreach (var button in _mainScreenButtons)
			{
				
					button.Item1.Background = Brushes.Transparent;
					button.Item1.IsEnabled = true;

					button.Item2.Source = (ImageSource)FindResource(button.Item3);
					button.Item5.Foreground = (Brush)FindResource("MyDarkGreen");
				
			}
		}

		private void JourneyDetailPage_UpdateJourney(int ID_Journey)
		{
			UpdateJourneyPage updateJourneyPage = new UpdateJourneyPage(ID_Journey);

			pageNavigation.NavigationService.Navigate(updateJourneyPage);
		}

		private void maximizeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.WindowState = WindowState.Maximized;
				iconMaximizeImage.Source = (ImageSource)FindResource("IconRestore");
			} 
			else
			{
				this.WindowState = WindowState.Normal;
				iconMaximizeImage.Source = (ImageSource)FindResource("IconMaximize");
			}
		}

		
	}
}
