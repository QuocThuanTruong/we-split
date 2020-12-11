using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using Microsoft.Maps.MapControl.WPF;
using WeSplit.Utilities;

namespace WeSplit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private BingMapUtilities _bingMapUtilities = BingMapUtilities.GetBingMapInstance();
        private GoogleMapUtilities _googleMapUtilities = GoogleMapUtilities.GetGoogleMapInstance();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task Test()
        {
            List<Location> locations = new List<Location> {
                new Location(10.883652174875964, 106.78150760740475),
                new Location(11.582181420032388, 108.07610286906946),
                new Location(11.93656053169823, 108.43766225557796),
                new Location(11.978014277359497, 107.67314865742867),
                new Location(12.282307089798318, 109.19050152674374),
                new Location(13.98055712175929, 108.00346968442308),
                new Location(10.403649799067841, 107.12466206906554)
            };

            var routeLine = await _bingMapUtilities.CreateDirectionInMap(locations);
            map.Children.Add(routeLine);

            for (int i = 1; i < locations.Count - 1; ++i)
            {
                var pin = _bingMapUtilities.CreateMarker(locations[i]);
                map.Children.Add(pin);
            }

            MapLayer mapLayerStart = _bingMapUtilities.CreateMarkerWithImage( "../../Assets/Images/start.jpg", locations[0]);
            map.Children.Add(mapLayerStart);


            MapLayer mapLayerEnd = _bingMapUtilities.CreateMarkerWithImage("../../Assets/Images/end.jpg", locations[locations.Count - 1]);
            map.Children.Add(mapLayerEnd); 
        }

        private void map_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            _ = Test();

            //var pin = _bingMapUtilities.CreateMarker(_googleMapUtilities.GetLocationByKeyName("KTX Khu B"));
            //map.Children.Add(pin);
        }
    }
}
