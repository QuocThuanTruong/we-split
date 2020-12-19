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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeSplit.Utilities;

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		public delegate void ViewAllJourneyHandler(int journeyStatus);
		public event ViewAllJourneyHandler ViewAllJourney;

		public delegate void ShowJourneyDetailPageHandler(int ID_Journey);
		public event ShowJourneyDetailPageHandler ShowJourneyDetailPage;

		public delegate void ShowJourneySearchPageHandler(string textSearch);
		public event ShowJourneySearchPageHandler ShowJourneySearchPage;


		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();

		const int PLANED = 1;
		const int CURRENT = 0;
		const int DONE = -1;

		private AppUtilities _appUtilities = AppUtilities.GetAppInstance();

		public HomePage()
		{
			InitializeComponent();

			loadPlanedJourneyInHomePageViewType1();
			loadDoneJourneyInHomePageViewType1();
			loadcurrentrentJourneyInHomePageViewType1();
			_databaseUtilities.GetListJourney();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			currentJourneyProgess.Value = 4;
		}

		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void viewAllJourneyButton_Click(object sender, RoutedEventArgs e)
		{
			//Adjust here
			ViewAllJourney?.Invoke(2);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewDetailButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void firstPlanedJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			firstPlanedJourneyViewDetailButton.Opacity = 1;
			firstPlanedJourneyButton.Opacity = 1;
			firstPlanedJourneyButton.FontSize = 6;
		}

		private void firstPlanedJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			firstPlanedJourneyViewDetailButton.Opacity = 0;
			firstPlanedJourneyButton.Opacity = 0;
		}

		private void secondPlanedJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			secondPlanedJourneyViewDetailButton.Opacity = 1;
			secondPlanedJourneyButton.Opacity = 1;
			secondPlanedJourneyButton.FontSize = 6;
		}

		private void secondPlanedJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			secondPlanedJourneyViewDetailButton.Opacity = 0;
			secondPlanedJourneyButton.Opacity = 0;
		}

		private void secondPlanedJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void firstPlanedJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}
		private void thirdPlanedJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void thirdPlanedJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			thirdPlanedJourneyViewDetailButton.Opacity = 1;
			thirdPlanedJourneyButton.Opacity = 1;
			thirdPlanedJourneyButton.FontSize = 6;
		}

		private void thirdPlanedJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			thirdPlanedJourneyViewDetailButton.Opacity = 0;
			thirdPlanedJourneyButton.Opacity = 0;
		}

		private void firstDoneJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			firstDoneJourneyViewDetailButton.Opacity = 1;
			firstDoneJourneyButton.Opacity = 1;
			firstDoneJourneyButton.FontSize = 6;
		}

		private void firstDoneJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			firstDoneJourneyViewDetailButton.Opacity = 0;
			firstDoneJourneyButton.Opacity = 0;
		}

		private void firstDoneJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void secondDoneJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			secondDoneJourneyViewDetailButton.Opacity = 1;
			secondDoneJourneyButton.Opacity = 1;
			secondDoneJourneyButton.FontSize = 6;
		}

		private void secondDoneJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			secondDoneJourneyViewDetailButton.Opacity = 0;
			secondDoneJourneyButton.Opacity = 0;
		}

		private void secondDoneJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void thirdDoneJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			thirdDoneJourneyViewDetailButton.Opacity = 1;
			thirdDoneJourneyButton.Opacity = 1;
			thirdDoneJourneyButton.FontSize = 6;
		}

		private void thirdDoneJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			thirdDoneJourneyViewDetailButton.Opacity = 0;
			thirdDoneJourneyButton.Opacity = 0;
		}

		private void thirdDoneJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void fourthDoneJourneyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			fourthDoneJourneyViewDetailButton.Opacity = 1;
			fourthDoneJourneyButton.Opacity = 1;
			fourthDoneJourneyButton.FontSize = 6;
		}

		private void fourthDoneJourneyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			fourthDoneJourneyViewDetailButton.Opacity = 0;
			fourthDoneJourneyButton.Opacity = 0;
		}

		private void fourthDoneJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

		private void loadPlanedJourneyInHomePageViewType1()
        {
			var sites = _databaseUtilities.GetSiteForBindingInHomePageView(PLANED);

			if (sites.Count >= 1)
            {
				var firstPlaned = sites[0];

				var firstPlanedAvatar = new BitmapImage();
				firstPlanedAvatar.BeginInit();
				firstPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;
				firstPlanedAvatar.UriSource = new Uri($"Images/Sites/{firstPlaned.ID_Site}.{firstPlaned.Site_Link_Avt}", UriKind.Relative);
				firstPlanedAvatar.EndInit();

				firstPlanedJourneyAvtImage.Source = firstPlanedAvatar;
				firstPlanedJourneyAvtHoverImage.Source = firstPlanedAvatar;
				firstPlanedJourneyRoute.Text = firstPlaned.Distance.ToString() + " km";
				firstPlanedJourneyTitle.Text = _appUtilities.getStandardName(firstPlaned.Site_Name, 22);
				firstPlanedJourneyViewDetailButton.Tag = firstPlaned.ID_Journey;

				if (sites.Count >= 2)
                {
					var secondPlaned = sites[1];

					var secondPlanedAvatar = new BitmapImage();
					secondPlanedAvatar.BeginInit();
					secondPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;

					secondPlanedAvatar.UriSource = new Uri($"Images/Sites/{secondPlaned.ID_Site}.{secondPlaned.Site_Link_Avt}", UriKind.Relative);

					secondPlanedAvatar.EndInit();

					secondPlanedJourneyAvtImage.Source = secondPlanedAvatar;
					secondPlanedJourneyAvtHoverImage.Source = secondPlanedAvatar;
					secondPlanedJourneyRoute.Text = secondPlaned.Distance.ToString() + " km";
					secondPlanedJourneyTitle.Text = _appUtilities.getStandardName(secondPlaned.Site_Name, 22);
					secondPlanedJourneyViewDetailButton.Tag = secondPlaned.ID_Journey;

					if (sites.Count >= 3)
                    {
						var thirdPlaned = sites[2];

						var thirdPlanedAvatar = new BitmapImage();
						thirdPlanedAvatar.BeginInit();
						thirdPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;

						thirdPlanedAvatar.UriSource = new Uri($"Images/Sites/{thirdPlaned.ID_Site}.{thirdPlaned.Site_Link_Avt}", UriKind.Relative);

						thirdPlanedAvatar.EndInit();

						thirdPlanedJourneyAvtImage.Source = thirdPlanedAvatar;
						thirdPlanedJourneyAvtHoverImage.Source = thirdPlanedAvatar;
						thirdPlanedJourneyRoute.Text = thirdPlaned.Distance.ToString() + " km";
						thirdPlanedJourneyTitle.Text = _appUtilities.getStandardName(thirdPlaned.Site_Name, 22);
						thirdPlanedJourneyViewDetailButton.Tag = thirdPlaned.ID_Journey;
					}
				}
			} 
			else
            {

            }
        }

		public void loadDoneJourneyInHomePageViewType1()
        {
			var sites = _databaseUtilities.GetSiteForBindingInHomePageView(DONE);

			if (sites.Count >= 1)
			{
				var firstDone = sites[0];

				var firstDoneAvatar = new BitmapImage();
				firstDoneAvatar.BeginInit();
				firstDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;

				firstDoneAvatar.UriSource = new Uri($"Images/Sites/{firstDone.ID_Site}.{firstDone.Site_Link_Avt}", UriKind.Relative);

				firstDoneAvatar.EndInit();

				firstDoneJourneyAvtImage.Source = firstDoneAvatar;
				firstDoneJourneyAvtHoverImage.Source = firstDoneAvatar;
				firstDoneJourneyRoute.Text = firstDone.Distance.ToString() + " km";
				firstDoneJourneyTitle.Text = _appUtilities.getStandardName(firstDone.Site_Name, 22);
				firstDoneJourneyViewDetailButton.Tag = firstDone.ID_Journey;

				if (sites.Count >= 2)
				{
					var secondDone = sites[1];

					var secondDoneAvatar = new BitmapImage();
					secondDoneAvatar.BeginInit();
					secondDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;

					secondDoneAvatar.UriSource = new Uri($"Images/Sites/{secondDone.ID_Site}.{secondDone.Site_Link_Avt}", UriKind.Relative);

					secondDoneAvatar.EndInit();

					secondDoneJourneyAvtImage.Source = secondDoneAvatar;
					secondDoneJourneyAvtHoverImage.Source = secondDoneAvatar;
					secondDoneJourneyRoute.Text = secondDone.Distance.ToString() + " km";
					secondDoneJourneyTitle.Text = _appUtilities.getStandardName(secondDone.Site_Name, 22);
					secondDoneJourneyViewDetailButton.Tag = secondDone.ID_Journey;

					if (sites.Count >= 3)
					{
						var thirdDone = sites[2];

						var thirdDoneAvatar = new BitmapImage();
						thirdDoneAvatar.BeginInit();
						thirdDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;

						thirdDoneAvatar.UriSource = new Uri($"Images/Sites/{thirdDone.ID_Site}.{thirdDone.Site_Link_Avt}", UriKind.Relative);

						thirdDoneAvatar.EndInit();

						thirdDoneJourneyAvtImage.Source = thirdDoneAvatar;
						thirdDoneJourneyAvtHoverImage.Source = thirdDoneAvatar;
						thirdDoneJourneyRoute.Text = thirdDone.Distance.ToString() + " km";
						thirdDoneJourneyTitle.Text = _appUtilities.getStandardName(thirdDone.Site_Name, 22);
						thirdDoneJourneyViewDetailButton.Tag = thirdDone.ID_Journey;

						if (sites.Count >= 4)
                        {
							var fourthDone = sites[3];

							var fourthDoneAvatar = new BitmapImage();
							fourthDoneAvatar.BeginInit();
							fourthDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;

							fourthDoneAvatar.UriSource = new Uri($"Images/Sites/{fourthDone.ID_Site}.{fourthDone.Site_Link_Avt}", UriKind.Relative);

							fourthDoneAvatar.EndInit();

							fourthDoneJourneyAvtImage.Source = fourthDoneAvatar;
							fourthDoneJourneyAvtHoverImage.Source = fourthDoneAvatar;
							fourthDoneJourneyRoute.Text = fourthDone.Distance.ToString() + " km";
							fourthDoneJourneyTitle.Text = _appUtilities.getStandardName(fourthDone.Site_Name, 22);
							fourthDoneJourneyViewDetailButton.Tag = fourthDone.ID_Journey;
						}
					}
				}
			}
		}

		private void loadcurrentrentJourneyInHomePageViewType1()
		{
			var current = _databaseUtilities.GetcurrentJourney();

			if (current != null)
            {
				var currentAvatar = new BitmapImage();
				currentAvatar.BeginInit();
				currentAvatar.CacheOption = BitmapCacheOption.OnLoad;
				currentAvatar.UriSource = new Uri($"Images\\Sites\\{current.ID_Site}.{_databaseUtilities.GetSiteForBindingInHomePageView(0).Single().Site_Link_Avt}", UriKind.Relative);
				currentAvatar.EndInit();

				currentJourneyAvt.Source = currentAvatar;
				currentJourneyName.Text = _appUtilities.getStandardName(current.Site_Name, 31);
				currentTotalJourneyDistance.Text = current.Distance.ToString() + "km";
				startDateTextBlock.Text = current.StartDate.Value.ToShortDateString();
				endDateTextBlock.Text = current.EndDate.Value.Date.ToShortDateString();
				currentJourneyProgess.Value = current.Journey_Progress;
				exploreCurrentJourneyButton.Tag = current.ID_Journey;
			}
		}

        private void exploreCurrentJourneyButton_Click(object sender, RoutedEventArgs e)
        {
			int ID_Journey = int.Parse(((Button)sender).Tag.ToString());

			ShowJourneyDetailPage?.Invoke(ID_Journey);
		}

        private void viewAllPlanedButton_Click(object sender, RoutedEventArgs e)
        {
			ViewAllJourney?.Invoke(PLANED);
		}

        private void viewAllDoneButton_Click(object sender, RoutedEventArgs e)
        {
			ViewAllJourney?.Invoke(DONE);
		}

		private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ShowJourneySearchPage?.Invoke(searchTextBox.Text);
			}	
		}
	}
}
