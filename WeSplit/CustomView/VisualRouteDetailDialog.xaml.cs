using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public void ShowDialog()
		{
			//Giả sử list lộ trình có 5 phần tử
			int routesLength = 5;

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
			HideDialog();
			CloseFullScreenVideoDialog?.Invoke();
		}

		private void map_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
