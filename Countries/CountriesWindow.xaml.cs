using Countries.Modelos;
using Countries.Servicos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Countries
{
    public partial class CountriesWindow : Window
    {
        private List<Country> listCountries;

        private CountryInfo listInfoCountries;

        private ApiService apiService;

        public string KeepUKText;
        public string KeepPTText;
        public CountriesWindow(List<Country> list)
        {
            InitializeComponent();

            apiService = new ApiService();

            listCountries = list;

            string baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Translation> TranslationList = new List<Translation>();
            TranslationList.Add(new Translation() { CountryName = "UnitedKingdom", Image = baseDir + "/Imagens/UnitedKingdom.png" });
            TranslationList.Add(new Translation() { CountryName = "Portugal", Image = baseDir + "/Imagens/Portugal.png" });

            cb_Language.ItemsSource = TranslationList;
            cb_Language.SelectedIndex = 0;

            Random random = new Random();
            int getRandom = random.Next(0, listCountries.Count);

            cb_Cnames.ItemsSource = listCountries;
            cb_Cnames.SelectedIndex = getRandom;
        }

        private async void cb_Cnames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Country country = cb_Cnames.SelectedItem as Country;

            var responseAPI = await apiService.GetInfoAPI("http://countriesinfo.somee.com", "/api/Countries/"+ country.name);

            listInfoCountries = (CountryInfo)responseAPI.Result;

            lbl_Cname.Content = country.name;
            if (string.IsNullOrEmpty(country.alpha2Code))
            {
                lbl_Alpha2Code.Content = "N/D";
            }
            else
            {
                lbl_Alpha2Code.Content = country.alpha2Code;
            }
            if (string.IsNullOrEmpty(country.alpha3Code))
            {
                lbl_Alpha3Code.Content = "N/D";
            }
            else
            {
                lbl_Alpha3Code.Content = country.alpha3Code;
            }
            if (string.IsNullOrEmpty(country.capital))
            {
                lbl_Capital.Content = "N/D";
            }
            else
            {
                lbl_Capital.Content = country.capital;
            }
            if (string.IsNullOrEmpty(country.region))
            {
                lbl_Region.Content = "N/D";
            }
            else
            {
                lbl_Region.Content = country.region;
            }
            if (string.IsNullOrEmpty(country.subregion))
            {
                lbl_Subregion.Content = "N/D";
            }
            else
            {
                lbl_Subregion.Content = country.subregion;
            }
            lbl_Population.Content = country.population;
            if (string.IsNullOrEmpty(country.demonym))
            {
                lbl_Demonym.Content = "N/D";
            }
            else
            {
                lbl_Demonym.Content = country.demonym;
            }
            if (string.IsNullOrEmpty(country.area))
            {
                lbl_Area.Content = "N/D";
            }
            else
            {
                lbl_Area.Content = country.area + " km²";
            }
            if (string.IsNullOrEmpty(country.gini))
            {
                lbl_Gini.Content = "N/D";
            }
            else
            {
                lbl_Gini.Content = country.gini;
            }
            if (string.IsNullOrEmpty(country.nativeName))
            {
                lbl_NativeName.Content = "N/D";
            }
            else
            {
                lbl_NativeName.Content = country.nativeName;
            }
            if (string.IsNullOrEmpty(country.numericCode))
            {
                lbl_numericCode.Content = "N/D";
            }
            else
            {
                lbl_numericCode.Content = country.numericCode;
            }
            if (string.IsNullOrEmpty(country.cioc))
            {
                lbl_Cioc.Content = "N/D";
            }
            else
            {
                lbl_Cioc.Content = country.cioc;
            }

            if (country.currencies != null)
            {
                UpdateListViews(country);
            }

            if (country.latlng != null)
            {
                UpdateGMap(country);
            }

            Uri resorceURI = new Uri(country.flag);

            img_flag.Source = new BitmapImage(resorceURI);

            if (listInfoCountries == null)
            {
                lbl_FoundationYear.Content = "N/D";
                txt_history.Text = "N/D";
                KeepUKText = "N/D";
                KeepPTText = "N/D";
            }
            else
            {
                lbl_FoundationYear.Content = listInfoCountries.FoundationYear;
                KeepUKText = listInfoCountries.CountryHistory;
                KeepPTText = listInfoCountries.CountryHistoryPT;
                if (cb_Language.SelectedIndex == 0)
                {
                    txt_history.Text = listInfoCountries.CountryHistory;
                }
                else
                {
                    txt_history.Text = listInfoCountries.CountryHistoryPT;
                }
            }
        }

        private void UpdateListViews(Country country)
        {
            lv_currency.ItemsSource = country.currencies;

            lv_timezones.ItemsSource = country.timezones;

            lv_borders.ItemsSource = country.borders;

            lv_Language.ItemsSource = country.languages;

            lv_topLevelDomain.ItemsSource = country.topLevelDomain;
        }
        private void UpdateGMap(Country country)
        {
            try
            {    
                GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

                // Coordenadas dos pais             
                mapView.Position = new GMap.NET.PointLatLng(country.latlng[0], country.latlng[1]);
                //
                mapView.Zoom = 6;
            }

            catch
            {
                GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.CacheOnly;
                mapView.Position = new GMap.NET.PointLatLng(0, 0);
                mapView.MinZoom = 1;
                mapView.Zoom = 1;
            }
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            mapView.MinZoom = 3;
            mapView.MaxZoom = 20;
            // whole world zoom
            mapView.Zoom = 6;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
        }

        private void cb_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Language.SelectedIndex == 0)
            {
                lbl_TitleCname.Content = "Country name: ";
                lbl_TitleNativeName.Content = "Native Name: ";
                lbl_titleDemonym.Content = "Demonym: ";
                lbl_TitleCapital.Content = "Capital: ";
                lbl_titleRegion.Content = "Region: ";
                lbl_TitleSubregion.Content = "Subregion: ";
                lbl_TitlePopulation.Content = "Population: ";
                lbl_TitlenumericCode.Content = "Numeric Code: ";
                lbl_TitleAlpha2Code.Content = "Alpha2Code: ";
                lbl_TitleAlpha3Code.Content = "Alpha3Code: ";
                lbl_TitleArea.Content = "Area: ";
                lbl_TitleGini.Content = "Gini: ";
                lbl_TitleCioc.Content = "Cioc: ";
                lbl_TitleCurrency.Content = "Currency: ";
                lbl_TitleTimeZones.Content = "Time Zones: ";
                lbl_TitleLanguage.Content = "Language: ";
                lbl_TitleTopLevelDomain.Content = "Top Level Domain: ";
                lbl_TitleBorders.Content = "Borders: ";
                lbl_changeLanguage.Content = "Change language: ";
                lbl_History.Content = "Country history: ";
                lbl_TitleFoundationYear.Content = "Foundation Year: ";
                btn_about.Content = "About";

                txt_history.Text = KeepUKText;
            }
            else if (cb_Language.SelectedIndex == 1)
            {
                lbl_TitleCname.Content = "Nome do país: ";
                lbl_TitleNativeName.Content = "Nome nativo: ";
                lbl_titleDemonym.Content = "Gentílico: ";
                lbl_TitleCapital.Content = "Capital: ";
                lbl_titleRegion.Content = "Região: ";
                lbl_TitleSubregion.Content = "Sub-região: ";
                lbl_TitlePopulation.Content = "População: ";
                lbl_TitlenumericCode.Content = "Código Numérico: ";
                lbl_TitleAlpha2Code.Content = "Código Alfa 2: ";
                lbl_TitleAlpha3Code.Content = "Código Alfa 3: ";
                lbl_TitleArea.Content = "Área: ";
                lbl_TitleGini.Content = "Índice de Gini: ";
                lbl_TitleCioc.Content = "Cioc: ";
                lbl_TitleCurrency.Content = "Moeda: ";
                lbl_TitleTimeZones.Content = "Fusos horários: ";
                lbl_TitleLanguage.Content = "Língua: ";
                lbl_TitleTopLevelDomain.Content = "Domínio: ";
                lbl_TitleBorders.Content = "Fronteiras: ";
                lbl_changeLanguage.Content = "Mudar idioma: ";
                lbl_History.Content = "História do pais: ";
                lbl_TitleFoundationYear.Content = "Ano de fundação: ";
                btn_about.Content = "Sobre";

                txt_history.Text = KeepPTText;
            }
        }

        private void MO_Gini(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
            if (cb_Language.SelectedIndex == 0)
            {
                tb_popuptext.Text = "Gini coefficient, sometimes called the Gini index or Gini ratio, is a measure of statistical dispersion intended to represent the income inequality or wealth inequality within a nation or any other group of people. It was developed by the Italian statistician and sociologist Corrado Gini.";
            }
            else if (cb_Language.SelectedIndex == 1)
            {
                tb_popuptext.Text = "Coeficiente de Gini, por vezes chamado índice de Gini ou razão de Gini, é uma medida de desigualdade desenvolvida pelo estatístico italiano Corrado Gini, e publicada no documento 'Variabilità e mutabilità', em 1912.";
            }
        }

        private void ML_Gini(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
        }

        private void btn_software_onclick(object sender, RoutedEventArgs e)
        {
            SoftwareWindow software = new SoftwareWindow();
            software.Show();
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void lbl_TitleAlpha2Code_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
            if (cb_Language.SelectedIndex == 0)
            {
                tb_popuptext.Text = "Alpha2 codes are two-letter country codes defined in ISO 3166-1, part of the ISO 3166 standard published by the International Organization for Standardization (ISO), to represent countries, dependent territories, and special areas of geographical interest.";
            }
            else if (cb_Language.SelectedIndex == 1)
            {
                tb_popuptext.Text = "Alfa2 são os códigos de países de duas-letras definido no ISO 3166-1, parte do padrão ISO 3166 publicado pela Organização Internacional para Padronização, para representar países, territórios dependentes, e de zonas especiais de interesse geográfico.";
            }
        }

        private void lbl_TitleAlpha2Code_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
        }

        private void lbl_TitleAlpha3Code_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
            if (cb_Language.SelectedIndex == 0)
            {
                tb_popuptext.Text = "Alpha3 codes are three-letter country codes defined in ISO 3166-1, part of the ISO 3166 standard published by the International Organization for Standardization (ISO), to represent countries, dependent territories, and special areas of geographical interest.";
            }
            else if (cb_Language.SelectedIndex == 1)
            {
                tb_popuptext.Text = "Alfa3 são os códigos de três-letras Código de país definido em ISO 3166-1, parte do padrão ISO 3166 publicado pela Organização Internacional para Padronização, para representar países, territórios dependentes, e zonas especiais de interesse geográfico.";
            }
        }

        private void lbl_TitleAlpha3Code_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
        }

        private void lbl_TitlenumericCode_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = true;
            if (cb_Language.SelectedIndex == 0)
            {
                tb_popuptext.Text = "ISO 3166-1 numeric (or numeric-3) codes are three-digit country codes defined in ISO 3166-1, part of the ISO 3166 standard published by the International Organization for Standardization (ISO), to represent countries, dependent territories, and special areas of geographical interest.";
            }
            else if (cb_Language.SelectedIndex == 1)
            {
                tb_popuptext.Text = "ISO 3166-1 numérico (ou numérico-3) os códigos são três dígitos, código de país é definido em ISO 3166-1, parte do ISO 3166 padrão publicado pela Organização Internacional para Padronização (ISO), para representar países, territórios dependentes, e de zonas especiais de interesse geográfico.";
            }
        }

        private void lbl_TitlenumericCode_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}
