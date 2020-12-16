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

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();

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
			ViewAllJourney?.Invoke(1);
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
			var sites = _databaseUtilities.GetSiteForBindingInHomePageView(1);

			if (sites.Count >= 1)
            {
				var firstPlaned = sites[0];

				var firstPlanedAvatar = new BitmapImage();
				firstPlanedAvatar.BeginInit();
				firstPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;
				firstPlanedAvatar.UriSource = new Uri(firstPlaned.Site_Link_Avt, UriKind.Relative);
				firstPlanedAvatar.EndInit();

				firstPlanedJourneyAvtImage.Source = firstPlanedAvatar;
				firstPlanedJourneyAvtHoverImage.Source = firstPlanedAvatar;
				firstPlanedJourneyRoute.Text = firstPlaned.Distance.ToString() + " km lộ trình";
				firstPlanedJourneyTitle.Text = firstPlaned.Site_Name;
				firstPlanedJourneyViewDetailButton.Tag = firstPlaned.ID_Journey;

				if (sites.Count >= 2)
                {
					var secondPlaned = sites[1];

					var secondPlanedAvatar = new BitmapImage();
					secondPlanedAvatar.BeginInit();
					secondPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;
					secondPlanedAvatar.UriSource = new Uri(secondPlaned.Site_Link_Avt, UriKind.Relative);
					secondPlanedAvatar.EndInit();

					secondPlanedJourneyAvtImage.Source = secondPlanedAvatar;
					secondPlanedJourneyAvtHoverImage.Source = secondPlanedAvatar;
					secondPlanedJourneyRoute.Text = secondPlaned.Distance.ToString() + " km lộ trình";
					secondPlanedJourneyTitle.Text = secondPlaned.Site_Name;
					secondPlanedJourneyViewDetailButton.Tag = secondPlaned.ID_Journey;

					if (sites.Count >= 3)
                    {
						var thirdPlaned = sites[2];

						var thirdPlanedAvatar = new BitmapImage();
						thirdPlanedAvatar.BeginInit();
						thirdPlanedAvatar.CacheOption = BitmapCacheOption.OnLoad;
						thirdPlanedAvatar.UriSource = new Uri(thirdPlaned.Site_Link_Avt, UriKind.Relative);
						thirdPlanedAvatar.EndInit();

						thirdPlanedJourneyAvtImage.Source = thirdPlanedAvatar;
						thirdPlanedJourneyAvtHoverImage.Source = thirdPlanedAvatar;
						thirdPlanedJourneyRoute.Text = thirdPlaned.Distance.ToString() + " km lộ trình";
						thirdPlanedJourneyTitle.Text = thirdPlaned.Site_Name;
						thirdPlanedJourneyViewDetailButton.Tag = thirdPlaned.ID_Journey;
					}
				}
			}
        }

		public void loadDoneJourneyInHomePageViewType1()
        {
			var sites = _databaseUtilities.GetSiteForBindingInHomePageView(1);

			if (sites.Count >= 1)
			{
				var firstDone = sites[0];

				var firstDoneAvatar = new BitmapImage();
				firstDoneAvatar.BeginInit();
				firstDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;
				firstDoneAvatar.UriSource = new Uri(firstDone.Site_Link_Avt, UriKind.Relative);
				firstDoneAvatar.EndInit();

				firstDoneJourneyAvtImage.Source = firstDoneAvatar;
				firstDoneJourneyAvtHoverImage.Source = firstDoneAvatar;
				firstDoneJourneyRoute.Text = firstDone.Distance.ToString() + " km lộ trình";
				firstDoneJourneyTitle.Text = firstDone.Site_Name;
				firstDoneJourneyViewDetailButton.Tag = firstDone.ID_Journey;

				if (sites.Count >= 2)
				{
					var secondDone = sites[1];

					var secondDoneAvatar = new BitmapImage();
					secondDoneAvatar.BeginInit();
					secondDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;
					secondDoneAvatar.UriSource = new Uri(secondDone.Site_Link_Avt, UriKind.Relative);
					secondDoneAvatar.EndInit();

					secondDoneJourneyAvtImage.Source = secondDoneAvatar;
					secondDoneJourneyAvtHoverImage.Source = secondDoneAvatar;
					secondDoneJourneyRoute.Text = secondDone.Distance.ToString() + " km lộ trình";
					secondDoneJourneyTitle.Text = secondDone.Site_Name;
					secondDoneJourneyViewDetailButton.Tag = secondDone.ID_Journey;

					if (sites.Count >= 3)
					{
						var thirdDone = sites[2];

						var thirdDoneAvatar = new BitmapImage();
						thirdDoneAvatar.BeginInit();
						thirdDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;
						thirdDoneAvatar.UriSource = new Uri(thirdDone.Site_Link_Avt, UriKind.Relative);
						thirdDoneAvatar.EndInit();

						thirdDoneJourneyAvtImage.Source = thirdDoneAvatar;
						thirdDoneJourneyAvtHoverImage.Source = thirdDoneAvatar;
						thirdDoneJourneyRoute.Text = thirdDone.Distance.ToString() + " km lộ trình";
						thirdDoneJourneyTitle.Text = thirdDone.Site_Name;
						thirdDoneJourneyViewDetailButton.Tag = thirdDone.ID_Journey;

						if (sites.Count >= 4)
                        {
							var fourthDone = sites[2];

							var fourthDoneAvatar = new BitmapImage();
							fourthDoneAvatar.BeginInit();
							fourthDoneAvatar.CacheOption = BitmapCacheOption.OnLoad;
							fourthDoneAvatar.UriSource = new Uri(fourthDone.Site_Link_Avt, UriKind.Relative);
							fourthDoneAvatar.EndInit();

							fourthDoneJourneyAvtImage.Source = fourthDoneAvatar;
							fourthDoneJourneyAvtHoverImage.Source = fourthDoneAvatar;
							fourthDoneJourneyRoute.Text = fourthDone.Distance.ToString() + " km lộ trình";
							fourthDoneJourneyTitle.Text = fourthDone.Site_Name;
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
				currentAvatar.UriSource = new Uri(current.Site_Avatar, UriKind.Relative);
				currentAvatar.EndInit();

				currentJourneyAvt.Source = currentAvatar;
				currentJourneyName.Text = current.Site_Name;
				currentTotalJourneyDistance.Text = current.Distance.ToString() + "km lộ trình";
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
    }
}
