using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;
using System.Diagnostics;

namespace WeSplit.Utilities
{
    class GoogleMapUtilities
    {
        private static string _googleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];

        private static GoogleMapUtilities _googleMapInstance;

        public static GoogleMapUtilities GetGoogleMapInstance()
        {
            if (_googleMapInstance == null)
            {
                _googleMapInstance = new GoogleMapUtilities();
            }
            else
            {
                //Do Nothing
            }

            return _googleMapInstance;
        }

        public Location GetLocationByKeyName(string keyName)
        {
            Location result = new Location();

            string GeocodingApi = ConfigurationManager.AppSettings["GeocodingApi"];

            string url = GeocodingApi + keyName + "&" + _googleMapApiKey;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(dataStream);

            string responseReader = streamReader.ReadToEnd();

            response.Close();

            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader(new StringReader(responseReader)));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["GeocodeResponse"].Rows[0]["status"].ToString() == "OK")
                {
                    result.Longitude = Convert.ToDouble(ds.Tables["location"].Rows[0]["lng"].ToString());
                    result.Latitude = Convert.ToDouble(ds.Tables["location"].Rows[0]["lat"].ToString());
                }
            }

            return result;
        }

    }
}
