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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeSplit.Utilities;

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for JourneyListPage.xaml
	/// </summary>
	public partial class JourneyListPage : Page
	{
		private List<Tuple<Image, TextBlock, string, string>> _statusGroups;
		public delegate void ShowJourneyDetailPageHandler(int ID_Journey);
		public event ShowJourneyDetailPageHandler ShowJourneyDetailPage;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();

		const int PLANED = 1;
		const int CURRENT = 0;
		const int DONE = 1;

		private int _journeyStatus = 2;

		private int _currentPage;
		private int _maxPage = 0;
		private bool _isFavorite = false;
		private int _typeGridCard = 0;
		private bool _isSearching = false;
		private string _prevCondition = "init";
		private bool _isFirstSearch = true;
		private int _sortedBy = 0;
		private bool _canSearchRequest = false;
		private string _search_text = "";
		private string _condition = "";

		private (string column, string type)[] _conditionSortedBy = {("StartDate", "DESC"), ("StartDate", "ASC"),
																	 ("EndDate", "ASC"), ("EndDate", "DESC"),
																	 ("Name", "DESC"), ("Name", "ASC")};
		public JourneyListPage()
		{
			InitializeComponent();
			filterContainer.Visibility = Visibility.Collapsed;

			loadJourneys();
		}

		public JourneyListPage(int journeyStatus)
		{
			InitializeComponent();
			filterContainer.Visibility = Visibility.Collapsed;

			_journeyStatus = journeyStatus;

			loadJourneys();
		}


		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_statusGroups = new List<Tuple<Image, TextBlock, string ,string>>()
			{
				new Tuple<Image, TextBlock, string, string>(doneStatusIcon, doneStatusTextBlock, "IconWhiteDone", "IconGreenDone"),
				new Tuple<Image, TextBlock, string, string>(currentStatusIcon, currentStatusTextBlock, "IconWhitecurrent", "IconGreencurrent"),
				new Tuple<Image, TextBlock, string, string>(planStatusIcon, planStatusTextBlock, "IconWhitePlan", "IconGreenPlan")
			};
		}

		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
			string search_text = searchTextBox.Text;

			if (search_text.Length != 0)
			{
				_isSearching = true;

				if (_isFirstSearch)
				{
					_currentPage = 1;
					_isFirstSearch = false;
				}

				string condition = "";

				if (_search_text != search_text || _condition != condition)
				{
					_search_text = search_text;
					_condition = condition;

					_canSearchRequest = true;
				}

				_condition = condition;

				if (_prevCondition != condition)
				{
					_currentPage = 1;
					_prevCondition = condition;
				}
				else
				{
					//Do Nothing
				}

				loadJourneySearch();
			}
			else
			{
				_isSearching = false;

				loadJourneys();
			}
		}

		private void sortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void filterButton_Click(object sender, RoutedEventArgs e)
		{
			if (filterContainer.Visibility == Visibility.Visible)
			{
				if (journeyGridView.Visibility == Visibility.Visible)
				{
					journeyGridView.SetValue(Grid.ColumnSpanProperty, 4);
				}

				if (journeyListView.Visibility == Visibility.Visible)
				{
					journeyListView.SetValue(Grid.ColumnSpanProperty, 4);
				}

				filterContainer.Visibility = Visibility.Collapsed;
			}
			else
			{
				if (journeyGridView.Visibility == Visibility.Visible)
				{
					journeyGridView.SetValue(Grid.ColumnSpanProperty, 2);
				}

				if (journeyListView.Visibility == Visibility.Visible)
				{
					journeyListView.SetValue(Grid.ColumnSpanProperty, 2);
				}

				filterContainer.Visibility = Visibility.Visible;
			}	
		}

		private void listViewButton_Click(object sender, RoutedEventArgs e)
		{
			listViewButton.Background = (Brush)FindResource("MyLightGreen2");
			listViewIcon.Source = (ImageSource)FindResource("IconListOn");

			gridViewButton.Background = Brushes.White;
			gridViewIcon.Source = (ImageSource)FindResource("IconGridOff");

			if (journeyListView.Visibility == Visibility.Visible)
			{
				journeyListView.Visibility = Visibility.Collapsed;
				journeyGridView.Visibility = Visibility.Visible;
			}	
			else
			{
				journeyListView.Visibility = Visibility.Visible;
				journeyGridView.Visibility = Visibility.Collapsed;

				if (filterContainer.Visibility == Visibility.Visible)
				{
					journeyListView.SetValue(Grid.ColumnSpanProperty, 2);
				}
			}	
		}

		private void gridViewButton_Click(object sender, RoutedEventArgs e)
		{
			gridViewButton.Background = (Brush)FindResource("MyLightGreen2");
			gridViewIcon.Source = (ImageSource)FindResource("IconGridOn");

			listViewButton.Background = Brushes.White;
			listViewIcon.Source = (ImageSource)FindResource("IconListOff");

			if (journeyGridView.Visibility == Visibility.Visible)
			{
				journeyGridView.Visibility = Visibility.Collapsed;
				journeyListView.Visibility = Visibility.Visible;
			}
			else
			{
				journeyGridView.Visibility = Visibility.Visible;
				journeyListView.Visibility = Visibility.Collapsed;

				if (filterContainer.Visibility == Visibility.Visible)
				{
					journeyGridView.SetValue(Grid.ColumnSpanProperty, 2);
				}
			}
		}

		private void journeyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedItemIndex = journeyGridView.SelectedIndex;
			int selectedID = -1;

			if (selectedItemIndex != -1)
			{
				selectedID = ((Journey)journeyGridView.SelectedItem).ID_Journey;
				Debug.WriteLine(selectedID);
			}

			//Get Id Journey base on item clikced
			ShowJourneyDetailPage?.Invoke(selectedID);
		}

		private void closeFilterButton_Click(object sender, RoutedEventArgs e)
		{
			if (journeyGridView.Visibility == Visibility.Visible)
			{
				journeyGridView.SetValue(Grid.ColumnSpanProperty, 4);
			}

			if (journeyListView.Visibility == Visibility.Visible)
			{
				journeyListView.SetValue(Grid.ColumnSpanProperty, 4);
			}

			filterContainer.Visibility = Visibility.Collapsed;
		}

		private void statusGroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach (var item in statusGroupListBox.SelectedItems)
			{
				var selectedButton = ((Button)item);

				selectedButton.Background = (Brush)FindResource("MyGreenGradient");

				_statusGroups[int.Parse(selectedButton.Tag.ToString())].Item1.Source = (ImageSource)FindResource(_statusGroups[int.Parse(selectedButton.Tag.ToString())].Item3);
				_statusGroups[int.Parse(selectedButton.Tag.ToString())].Item2.Foreground = Brushes.White;
			}
		}

		private void groupButton_Click(object sender, RoutedEventArgs e)
		{
			//Convert currentrent clicked button to list item
			var clickedButton = ((Button)sender);
			var clickedItemIdx = int.Parse(clickedButton.Tag.ToString());
			var clickedItem = statusGroupListBox.Items.GetItemAt(clickedItemIdx);

			//Add this converted item if new else remove it
			if (statusGroupListBox.SelectedItems.Contains(clickedItem))
			{
				statusGroupListBox.SelectedItems.Remove(clickedItem);
				clickedButton.Background = (SolidColorBrush)FindResource("MyLightGreenOpacity");

				_statusGroups[clickedItemIdx].Item1.Source = (ImageSource)FindResource(_statusGroups[clickedItemIdx].Item4);
				_statusGroups[clickedItemIdx].Item2.Foreground = (SolidColorBrush)FindResource("MyNeueGreen");
			}
			else
			{
				statusGroupListBox.SelectedItems.Add(clickedItem);
			}


			Debug.WriteLine(((Button)sender).Tag.ToString());

			foreach (var item in statusGroupListBox.SelectedItems)
			{

				Debug.WriteLine(((Button)item).Name);
			}
		}

		private void prevPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void firstPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void nextPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void lastPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void loadJourneys()
        {
			List<Journey> journeys;

			if (_journeyStatus != 2)
            {
				journeys = _databaseUtilities.GetListJourneyByStatus(_journeyStatus);
            } else
            {
				journeys = _databaseUtilities.GetListJourney();
            }

			journeyGridView.ItemsSource = journeys;
			journeyListView.ItemsSource = journeys;
		}

		private void loadJourneySearch()
		{
			(List<Journey> Journeys, int totalJourneyResult) JourneysSearchResults = _databaseUtilities.GetJourneySearchResult(_search_text, _condition, _conditionSortedBy[_sortedBy], _currentPage, 10);

			_maxPage = 10;
            if (_maxPage == 0)
            {
                _maxPage = 1;
                _currentPage = 1;

               // messageNotFoundContainer.Visibility = Visibility.Visible;
            }
            else
            {
               // messageNotFoundContainer.Visibility = Visibility.Hidden;
            }

            currentPageTextBlock.Text = $"{_currentPage} of {(_maxPage)}";

            List<Journey> Journeys = JourneysSearchResults.Journeys;
            if (Journeys.Count > 0)
            {
                journeyGridView.ItemsSource = Journeys;
				journeyListView.ItemsSource = Journeys;

                currentResultTextBlock.Text = $"Hiển thị {Journeys.Count} trong tổng số {JourneysSearchResults.totalJourneyResult} món ăn";
            }
            else
            {
				journeyGridView.ItemsSource = null;
				journeyListView.ItemsSource = null;
                currentResultTextBlock.Text = "Không tìm thấy món ăn thỏa yêu cầu";
            }
        }
    }
}
