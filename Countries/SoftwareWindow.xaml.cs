using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Countries
{
    /// <summary>
    /// Interaction logic for SoftwareWindow.xaml
    /// </summary>
    public partial class SoftwareWindow : Window
    {
        public SoftwareWindow()
        {
            InitializeComponent();

            InitProgress();
        }

        private async Task InitProgress()
        {
            var progress = new Progress<int>(value => SoftwareProgress.Value = value);
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    ((IProgress<int>)progress).Report(i);
                    Thread.Sleep(100);
                }
            });
            this.Close();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
