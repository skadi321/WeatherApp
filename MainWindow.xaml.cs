using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherApp
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

        private async void  TextBlockVarazdin_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockVarazdinTemperature.Text = "Loading";

            while (true)
            {
                JArray result = await KafkaReceiver.RecieveTemperatureMessageAsync();
                Dispatcher.Invoke(() =>
                {
                    for (int i = 1; i < 3; i++)
                    {


                        switch (result[i]["row"]["columns"][0].ToString())
                        {
                            case "Varazdin":
                                TextBlockVarazdinTemperature.Text = result[i]["row"]["columns"][1].ToString();
                                break;
                            case "Zagreb":
                                TextBlockZagrebTemperature.Text = result[i]["row"]["columns"][1].ToString();
                                break;
                            case "Split":
                                TextBlockSplitTemperature.Text = result[i]["row"]["columns"][1].ToString();
                                break;
                            case "Rijeka":
                                TextBlockRijekaTemperature.Text = result[i]["row"]["columns"][1].ToString();
                                break;
                            default:
                                break;
                        }
                    }

                });
            }

            
            
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
