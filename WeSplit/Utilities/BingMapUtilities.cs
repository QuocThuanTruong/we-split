using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using BingMapsRESTToolkit;
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace WeSplit.Utilities
{
    class BingMapUtilities
    {
        private static string _bingMapApiKey = ConfigurationManager.AppSettings["BingMapApiKey"];

        private static BingMapUtilities _bingMapInstance;

        public static BingMapUtilities GetBingMapInstance()
        {
            if (_bingMapInstance == null)
            {
                _bingMapInstance = new BingMapUtilities();
            }
            else
            {
                //Do Nothing
            }

            return _bingMapInstance;
        }

        public Pushpin CreateMarker(Microsoft.Maps.MapControl.WPF.Location location)
        {
            Pushpin pin = new Pushpin{ Location = location };

            return pin;
        }

        public Pushpin CreateMarker(Double longtitude, Double latitude)
        {
            Pushpin pin = new Pushpin();

            pin.Location.Longitude = longtitude;
            pin.Location.Latitude = latitude;

            return pin;
        }

        public MapLayer CreateMarkerWithImage(string imageUri, Microsoft.Maps.MapControl.WPF.Location location)
        {
            MapLayer result = new MapLayer();

            Image imageStart = new Image();
            imageStart.Source = new BitmapImage(new Uri(imageUri, UriKind.Relative));
            imageStart.Width = 50;
            imageStart.Height = 50;

            result.AddChild(imageStart, location);

            return result;
        }

        public async Task<MapPolyline> CreateDirectionInMap(List<Microsoft.Maps.MapControl.WPF.Location> locationsFromDB)
        {
            var waypoints = new List<SimpleWaypoint>();

            foreach(var location in locationsFromDB)
            {
                var waypoint = new SimpleWaypoint();
                waypoint.Coordinate = new Coordinate();

                waypoint.Coordinate.Longitude = location.Longitude;
                waypoint.Coordinate.Latitude = location.Latitude;

                waypoints.Add(waypoint);
            }

            MapPolyline result = await CreateRoute(waypoints);

            return result;
        }

        public async Task<MapPolyline> CreateRoute(List<SimpleWaypoint> waypoints)
        {
            MapPolyline result = new MapPolyline();

            var request = new RouteRequest()
            {
                RouteOptions = new RouteOptions()
                {
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath
                    }
                },
                Waypoints = waypoints,
                BingMapsKey = _bingMapApiKey
            };

            var response = await ServiceManager.GetResponseAsync(request);

            if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {
                var route = response.ResourceSets[0].Resources[0] as Route;
                var coords = route.RoutePath.Line.Coordinates;
                var locations = new LocationCollection();

                for (int i = 0; i < coords.Length; i++)
                {
                    locations.Add(new Microsoft.Maps.MapControl.WPF.Location(coords[i][0], coords[i][1]));
                }

                result.Locations = locations;
                result.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
                result.StrokeThickness = 5;
            }

            return result;
        }

    }
}
