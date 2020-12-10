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

namespace WeSplit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GO_Click(object sender, RoutedEventArgs e)
        {
            string DistanceApiUrl = ConfigurationManager.AppSettings["DistanceApi"];
            string API_KEY = ConfigurationManager.AppSettings["ApiKey"];

            string srcText = Src.Text;
            string desText = Des.Text;

            string url = DistanceApiUrl + "&origins=" + srcText + "&destinations=" + desText + "&mode=driving&sensor=false&language=vi-VI&units=imperial&key=" + API_KEY;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader(new StringReader(responsereader)));
            if (ds.Tables.Count > 0)
            {
                var duration = "";
                var distance = "";

                if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
                {
                    duration = Convert.ToString(ds.Tables["duration"].Rows[0]["text"].ToString().Trim());
                    distance = Convert.ToString(ds.Tables["distance"].Rows[0]["text"].ToString());
                }

                Duration.Text = duration;
                Distance.Text = Convert.ToString(distance);

                map.Navigate("https://www.google.co.in/maps/dir/" + srcText + "/" + desText);
            }
        }
    }
}
