using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using WeSplit.Utilities;

namespace WeSplit.Pages
{
	/// <summary>
	/// Interaction logic for AddSitePage.xaml
	/// </summary>
	public partial class AddSitePage : Page
	{
		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDBInstance();
		private AppUtilities _appUtilities = AppUtilities.GetAppInstance();

		private List<Site> _sites;
		private List<Province> _provinces;
		private string _srcAvatarSite = "";
		public AddSitePage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_sites = _databaseUtilities.GetListSite();
			_provinces = _databaseUtilities.GetListProvince();

			sitesListView.ItemsSource = _sites;
			startProvinceComboBox.ItemsSource = _provinces;
		}

		private void addSiteAvatarButton_Click(object sender, RoutedEventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.ico)|*.png;*.jpeg;*.jpg;*.ico";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					_srcAvatarSite = openFileDialog.FileName;

					BitmapImage bitmap = new BitmapImage();

					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Relative);
					bitmap.EndInit();

					avatarImage.Source = bitmap;
					avatarImage.Visibility = Visibility.Visible;

					addSiteAvatarButton.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void addSiteButton_Click(object sender, RoutedEventArgs e)
		{
			Site site = new Site();

			site.ID_Site = _databaseUtilities.GetMaxIDSite() + 1;
			site.ID_Province = ((Province)startProvinceComboBox.SelectedItem).ID_Province;

			site.Site_Name = siteNameTextBox.Text;
			if (site.Site_Name.Length == 0)
            {
				return;
            }

			site.Site_Address = siteAddressTextBox.Text;
			if (site.Site_Address.Length == 0)
            {
				return;
            }

			if (_srcAvatarSite == "")
			{
				return;
			}

			site.Site_Description = siteDescriptionTextBox.Text;
			if (site.Site_Description.Length == 0)
            {
				return;
            }

			site.Site_Link_Avt = _appUtilities.getTypeOfImage(_srcAvatarSite);

			//Add
			_databaseUtilities.AddNewSite(site.ID_Site, site.ID_Province, site.Site_Name, site.Site_Description, site.Site_Link_Avt, site.Site_Address);

			_appUtilities.createSitesDirectory();
			_appUtilities.copyImageToIDirectory(site.ID_Site, _srcAvatarSite, "", true);

			_sites.Add(site);

			//Reset Input
			siteNameTextBox.Text = "";
			siteAddressTextBox.Text = "";
			siteDescriptionTextBox.Text = "";

			sitesListView.ItemsSource = null;
			sitesListView.ItemsSource = _sites;

			avatarImage.Visibility = Visibility.Collapsed;
			addSiteAvatarButton.Visibility = Visibility.Visible;
			startProvinceComboBox.SelectedIndex = 0;
		}

		private void cancelAddSiteButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
