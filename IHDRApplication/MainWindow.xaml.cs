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
using MNISTParserLib;
using IHDRLib;

namespace IHDRApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IHDR ihdr;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MnistParser parser = new MnistParser(@"C:\Users\YoYo\Desktop\IHDRApplication\Data\train_images.bin", @"C:\Users\YoYo\Desktop\IHDRApplication\Data\train_labels.bin");
            parser.ParseData(1200);

            ihdr = new IHDR();
            List<MNISTParserLib.Sample> mnistSamples = parser.Samples;
            foreach (var item in mnistSamples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MnistParser parser = new MnistParser(@"C:\Users\YoYo\Desktop\IHDRApplication\Data\train_images.bin", @"C:\Users\YoYo\Desktop\IHDRApplication\Data\train_labels.bin");
            parser.ParseData(1);

            parser.SaveSamplesToBmp(@"D:\Samples");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ihdr.BuildTree();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ihdr.SaveTreeToFileHierarchy();
        }

        //private void Button_Click_3(object sender, RoutedEventArgs e)
        //{
            //double averageDistance = ihdr.Samples.GetAverageDisanceBetweenSamples();
            //double maxDistance = ihdr.Samples.GetMaxDisanceBetweenSamples();
            //MessageBox.Show(averageDistance.ToString());
            //Console.WriteLine("Label 0 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(0).ToString());
            //Console.WriteLine("Label 1 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(1).ToString());
            //Console.WriteLine("Label 2 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(2).ToString());
            //Console.WriteLine("Label 3 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(3).ToString());
            //Console.WriteLine("Label 4 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(4).ToString());
            //Console.WriteLine("Label 5 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(5).ToString());
            //Console.WriteLine("Label 6 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(6).ToString());
            //Console.WriteLine("Label 7 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(7).ToString());
            //Console.WriteLine("Label 8 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(8).ToString());
            //Console.WriteLine("Label 9 : " + ihdr.Samples.GetAverageDisanceBetweenSamplesOfOneLabel(9).ToString());
        //}
    }
}
