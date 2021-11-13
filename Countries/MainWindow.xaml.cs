using Countries.Modelos;
using Countries.Servicos;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Countries
{
    
    public partial class MainWindow : Window
    {
        #region Atributos
        private List<Country> listofCountries;

        private NetworkService networkService;

        private ApiService apiService;

        private DialogService dialogService;

        private DataService dataService;

        private DownloadService downloadService;
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            networkService = new NetworkService();
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            downloadService = new DownloadService();

            LoadCountries();
        }

        private async Task LoadCountries()
        {
            bool load;

            var progressReport = new Progress<int>(ProcessingProgress);

            string baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            baseDir = baseDir + "\\Imagens\\";

            lbl_mensagem.Content = "Checking internet connection...";

            var connection = networkService.CheckConnection();

            if (!connection.IsSucess)
            {
                //Se não houver ligação a internet vai para Base de dados
                lbl_mensagem.Content = "Getting information from database...";

                //substituir caminho da foto
                ImageSource imageSource = new BitmapImage(new Uri(@baseDir+ "\\icons8_red_circle_25px.png"));
                img_Internet.Source = imageSource;
                lbl_Internet.Content = "Internet connection failed!";

                ImageSource imageAPISource = new BitmapImage(new Uri(@baseDir + "\\icons8_red_circle_25px.png"));
                img_Api.Source = imageAPISource;
                lbl_Api.Content = "API connection failed!";

                LoadLocalCountries();
                load = false;
            }
            else
            {
                //Ligação feita com sucesso 

                //substituir caminho da foto
                ImageSource imageSource = new BitmapImage(new Uri(@baseDir + "\\icons8_green_circle_25px.png"));
                img_Internet.Source = imageSource;
                lbl_Internet.Content = "Internet connection successfully made!";
                await LoadApiInfo(progressReport);
                load = true;

                ImageSource imageAPISource = new BitmapImage(new Uri(@baseDir + "\\icons8_green_circle_25px.png"));
                img_Api.Source = imageAPISource;
                lbl_Api.Content = "API connection successfully made!";

                ImageSource imageDataSource = new BitmapImage(new Uri(@baseDir + "\\icons8_green_circle_25px.png"));
                img_Data.Source = imageDataSource;
                lbl_Data.Content = "Data base connection successfully upload!";

                await DownloadFlags(progressReport);

                await ConvertImages(progressReport);

                await dataService.DeleteData();
                await dataService.SaveData(listofCountries);
            }

            if (listofCountries.Count == 0)
            {
                lbl_mensagem.Content = "No Internet connection detected!" + Environment.NewLine + "And the country information was not previously loaded" + Environment.NewLine + "Try later!";

                ImageSource imageDataSource = new BitmapImage(new Uri(@baseDir + "\\icons8_red_circle_25px.png"));
                img_Data.Source = imageDataSource;
                lbl_Data.Content = "Data base connection failed!";
            }
            

            if (load)
            {
                lbl_mensagem.Content = "All information has been successfully loaded!";

                btn_enter.Visibility = Visibility.Visible;
            }
            else
            {
                var progress = new Progress<int>(value => pgProgress.Value = value);
                await Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        ((IProgress<int>)progress).Report(i);
                        Thread.Sleep(20);
                    }
                });
                ImageSource imageDataSource = new BitmapImage(new Uri(@baseDir + "\\icons8_green_circle_25px.png"));
                img_Data.Source = imageDataSource;
                lbl_Data.Content = "Data base connection successfully made!";

                lbl_mensagem.Content = "Information has been successfully loaded from Data Base!";

                btn_enter.Visibility = Visibility.Visible;
            }
        }

        private void ProcessingProgress(int percentage)
        {
            pgProgress.Value = percentage;
        }

        private async Task ConvertImages(IProgress<int> progress)
        {
            await Task.Run(() =>
            {
                int taskResoved = 0;

                foreach (Country country in listofCountries)
                {

                    string baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    string pathSVG = baseDir + "\\Flags\\" + country.name + ".svg";
                    string pathPNG = baseDir + "\\Flags\\" + country.name + ".png";
                    string pathNoFlag = baseDir + "\\Flags\\" + "BandeiraBranca.png";


                    // Settings
                    MagickReadSettings settings = new MagickReadSettings();
                    settings.Width = 200;
                    settings.Height = 100;


                    // Crear imagem
                    MagickImage imagemPng = new MagickImage(pathSVG, settings);

                    // Convrter em formato png
                    imagemPng.Format = MagickFormat.Png;

                    // Grava imagem convertida
                    imagemPng.Write(pathPNG);

                    if (!string.IsNullOrEmpty(country.flag))
                    {
                        country.flag = pathPNG;
                    }
                    else
                    {
                        country.flag = pathNoFlag;
                    }
                    

                    if (progress != null)
                    {
                        taskResoved++;

                        var percentage = (double)taskResoved / listofCountries.Count;

                        percentage = percentage * 100;

                        var percentageInt = (int)Math.Round(percentage, 0);

                       progress.Report(percentageInt);
                    }
                }
            });
        }

        private async Task DownloadFlags(IProgress<int> progress)
        {
            int taskResoved = 0;
            //Criar pasta Imagens
            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");
            }
            lbl_mensagem.Content = "Downloading country flags...";

            await downloadService.GetFlags(listofCountries);
            if (progress != null)
            {
                taskResoved++;

                progress.Report(taskResoved);
            }
        }

        private async Task LoadApiInfo(IProgress<int> progress)
        {
            int taskResoved = 0;

            lbl_mensagem.Content = "Waiting for the API connection...";

            var response = await apiService.GetInfo("https://restcountries.eu", "/rest/v2/all");

            listofCountries = (List<Country>)response.Result;

            if (progress != null)
            {
                taskResoved++;

                progress.Report(taskResoved);
            }
        }

        private void LoadLocalCountries()
        {
            listofCountries = dataService.GetData();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            CountriesWindow win2 = new CountriesWindow(listofCountries);
            win2.Show();
            this.Close();
        }
    }
}
