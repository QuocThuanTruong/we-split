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

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		private Duration blurRadius;

		public HomePage()
		{
			InitializeComponent();
		}

		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void sortBoxContainer_Click(object sender, RoutedEventArgs e)
		{

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

		}

		private void firstPlanedJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{

		}
		private void thirdPlanedJourneyViewDetailButton_Click(object sender, RoutedEventArgs e)
		{

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

		}
	}
}
