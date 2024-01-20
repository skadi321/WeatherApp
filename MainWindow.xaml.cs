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
            Task.Run(async () => 
            { 
                while (true)
                {
                    Thread.Sleep(2000);
                    await KafkaProducer.SendTemperatureMessageAsync();
                    
                }
                 }
            );
        }

        private async void  TextBlockVarazdin_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockVarazdinTemperature.Text = "Loading";
            await Task.Run(async () =>
            {
                while (true)
                {
                    JArray result = await KafkaReceiver.RecieveTemperatureMessageAsync();
                    Dispatcher.Invoke(() =>
                    {
                        switch (result[1]["row"]["columns"][0].ToString())
                        {
                            case "Varazdin":
                                TextBlockVarazdinTemperature.Text = result[1]["row"]["columns"][1].ToString();
                                break;
                            case "Zagreb":
                                TextBlockZagrebTemperature.Text = result[1]["row"]["columns"][1].ToString();
                                break;
                            case "Split":
                                TextBlockSplitTemperature.Text = result[1]["row"]["columns"][1].ToString();
                                break;
                            case "Rijeka":
                                TextBlockRijekaTemperature.Text = result[1]["row"]["columns"][1].ToString();
                                break;
                        }
                       });
                }

            });
            
        }
    }
}
