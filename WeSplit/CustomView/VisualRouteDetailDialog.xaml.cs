using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WeSplit.Utilities;
using Microsoft.Maps.MapControl.WPF;

namespace WeSplit.CustomView
{
	/// <summary>
	/// Interaction logic for VisualRouteDetailDialog.xaml
	/// </summary>
	public partial class VisualRouteDetailDialog : UserControl
	{
		private bool _hideRequest = false;
		private UIElement _parent;

		public delegate void CloseFullScreenVideoDialogHandler();
		public event CloseFullScreenVideoDialogHandler CloseFullScreenVideoDialog;

		private ObservableCollection<Tuple<int, VerticalAlignment>> _borderMilestones = new ObservableCollection<Tuple<int, VerticalAlignment>>();

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();
		private GoogleMapUtilities _googleMapUtilities = GoogleMapUtilities.GetGoogleMapInstance();
		private BingMapUtilities _bingMapUtilities = BingMapUtilities.GetBingMapInstance();
		private MapPolyline routeLine;

		public VisualRouteDetailDialog()
		{
			InitializeComponent();

			Visibility = Visibility.Collapsed;
		}

		public void SetParent(UIElement parent)
		{
			_parent = parent;
		}

		//Params will define depend on your need
		public void ShowDialog(List<Route> routes, Route startRoute, Route endRoute)
		{
			startRoute.Route_Detail = startRoute.Place + ", " + startRoute.Province;
			Location startRouteLocation = _googleMapUtilities.GetLocationByKeyName(startRoute.Route_Detail);

			endRoute.Route_Detail = endRoute.Place + ", " + endRoute.Province;
			Location endRouteLocation = _googleMapUtilities.GetLocationByKeyName(endRoute.Route_Detail);

			List<Location> locations = new List<Location>();
			locations.Add(startRouteLocation);

			routes.Insert(0, startRoute);
			routes.Add(endRoute);

			routeDetailListView.ItemsSource = routes;

			for (int i = 0; i < routes.Count; ++i)
			{
				routes[i].Route_Detail = routes[i].Place + ", " + routes[i].Province;

				Location location = _googleMapUtilities.GetLocationByKeyName(routes[i].Route_Detail);
				locations.Add(location);

				routeMap.Children.Add(_bingMapUtilities.CreateMarkerWithImage(FindResource("MarkerSite").ToString(), location));
			}

			locations.Add(endRouteLocation);

			_ = CreateDirection(locations);

			routeMap.Children.Add(_bingMapUtilities.CreateMarkerWithImage(FindResource("MarkerStart").ToString(), startRouteLocation));
			routeMap.Children.Add(_bingMapUtilities.CreateMarkerWithImage(FindResource("MarkerEnd").ToString(), endRouteLocation));

			routeMap.ZoomLevel = 8;
			routeMap.Center.Longitude = (startRouteLocation.Longitude + endRouteLocation.Longitude);
			routeMap.Center.Latitude = (startRouteLocation.Latitude + endRouteLocation.Latitude);

			//Giả sử list lộ trình có 5 phần tử
			int routesLength = routes.Count;

			_borderMilestones.Add(new Tuple<int, VerticalAlignment>(30, VerticalAlignment.Bottom));

			for (int i = 1; i < routesLength - 1; i++)
			{
				_borderMilestones.Add(new Tuple<int, VerticalAlignment>(60, VerticalAlignment.Center));
			}

			_borderMilestones.Add(new Tuple<int, VerticalAlignment>(30, VerticalAlignment.Top));


			routeMilestoneListView.ItemsSource = _borderMilestones;

			_parent.IsEnabled = false;
			_hideRequest = false;

			Visibility = Visibility.Visible;

			while (!_hideRequest)
			{
				//Stop if app close
				if (this.Dispatcher.HasShutdownStarted || this.Dispatcher.HasShutdownFinished)
				{
					break;
				}

				this.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
				Thread.Sleep(20);
			}

			
		}

		public void HideDialog()
		{
			_hideRequest = true;
			Visibility = Visibility.Collapsed;
			_parent.IsEnabled = true;
		}

		private void closeDialogButton_Click(object sender, RoutedEventArgs e)
		{
			routeDetailListView.ItemsSource = null;
			routeMilestoneListView.ItemsSource = null;
			_borderMilestones = new ObservableCollection<Tuple<int, VerticalAlignment>>();

			routeMap.Children.Clear();

			HideDialog();
			CloseFullScreenVideoDialog?.Invoke();
		}

		private void map_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void routeDetailListView_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
		{
			e.Handled = true;
		}

		private async Task CreateDirection(List<Location> locations)
		{
			routeLine = await _bingMapUtilities.CreateDirectionInMap(locations);

			routeMap.Children.Add(routeLine);
		}
	}
}
