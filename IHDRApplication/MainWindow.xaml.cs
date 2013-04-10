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
using S = SyntheticDataGenerator;
using IHDRLib;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using ArceneParserLib;
using GisetteParserLib;
using FacesParserLib;
using Faces2ParserLib;

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
            MnistParser parser = new MnistParser(
                    @"D:\Dropbox\DP\data\train-images.bin",
                    @"D:\Dropbox\DP\data\train-labels.bin",
                    @"D:\Dropbox\DP\data\test-images.bin",
                    @"D:\Dropbox\DP\data\test-labels.bin");

            parser.ParseData(5000);
            parser.ParseDataTest(1000);

            ihdr = new IHDR();
            List<MNISTParserLib.Sample> mnistSamples = parser.Samples;
            foreach (var item in mnistSamples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
            foreach (var item in parser.SamplesTest)
            {
                ihdr.AddTestingSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MnistParser parser = new MnistParser(
                   @"D:\Dropbox\DP\data\train-images.bin",
                   @"D:\Dropbox\DP\data\train-labels.bin",
                   @"D:\Dropbox\DP\data\test-images.bin",
                   @"D:\Dropbox\DP\data\test-labels.bin"
                   );

            parser.ParseData(3000);
            parser.ParseDataTest(2000);

            parser.SaveSamplesToBmp(@"D:\Samples\Train");
            parser.SaveTestSamplesToBmp(@"D:\Samples\Test");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ihdr.BuildTree_MNIST_MyOutput();
            Console.WriteLine(ihdr.ResultMessage);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ihdr.SaveTreeToFileHierarchy();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();

            FileStream s = new FileStream(@"C:\IHDRSerializedTree.txt", FileMode.Create);
            formatter.Serialize(s, ihdr);
            s.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream s = new FileStream(@"C:\IHDRSerializedTree.txt", FileMode.Open);
            ihdr = (IHDR)formatter.Deserialize(s);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            S.SyntheticDataGenerator generator = new S.SyntheticDataGenerator(70, 50);
            generator.GenerateSyntheticData();
            generator.GenerateSyntheticDataTest();

            ihdr = new IHDR();
            foreach (var item in generator.samples)
            {
                ihdr.AddSample(item.Attributes.ToArray(), item.AttributesY.ToArray(), item.Label);
            }
            foreach (var item in generator.samplesTest)
            {
                ihdr.AddTestingSample(item.Attributes.ToArray(), (double)item.Label);
            }

            ihdr.BuildTree_Synthetic();
            ihdr.EvaluateClustersLabels();
            ihdr.ExecuteTesting();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            S.SyntheticDataGenerator generator = new S.SyntheticDataGenerator(70, 50);
            generator.GenerateSyntheticData();
            generator.GenerateSyntheticDataTest();

            ihdr = new IHDR();
            foreach (var item in generator.samples)
            {
                ihdr.AddSample(item.Attributes.ToArray(), item.AttributesY.ToArray(), item.Label);
            }
            foreach (var item in generator.samplesTest)
            {
                ihdr.AddTestingSample(item.Attributes.ToArray(), (double)item.Label);
            }

            ihdr.BuildTree_Synthetic();
            ihdr.EvaluateClustersLabels();
            ihdr.SaveLeafClustersToPicture();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            S.SyntheticDataGenerator generator = new S.SyntheticDataGenerator(100, 100);
            generator.GenerateSyntheticData();
            generator.GenerateSyntheticDataTest();

            ihdr = new IHDR();
            foreach (var item in generator.samples)
            {
                ihdr.AddSample(item.Attributes.ToArray(), item.AttributesY.ToArray(), item.Label);
            }
            foreach (var item in generator.samplesTest)
            {
                ihdr.AddTestingSample(item.Attributes.ToArray(), (double)item.Label);
            }

            ihdr.BuildTree_Synthetic();
            ihdr.EvaluateAllClustersLabels();
            ihdr.EvaluateDepth();
            ihdr.SaveLayersToBmp(@"D:\Levels");
        }

        // arcane initialization
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            ArceneParser parser = new ArceneParser(
                    @"D:\Dropbox\DP\datasets\Arcene\arcene_train.data",
                    @"D:\Dropbox\DP\datasets\Arcene\arcene_train.labels",
                    @"D:\Dropbox\DP\datasets\Arcene\arcene_valid.data",
                    @"D:\Dropbox\DP\datasets\Arcene\arcene_valid.labels");

            parser.ParseData();
            parser.ParseDataTest();

            ihdr = new IHDR();
            List<ArceneParserLib.Sample> arceneSamples = parser.Samples;
            foreach (var item in arceneSamples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
            foreach (var item in parser.SamplesTest)
            {
                ihdr.AddTestingSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            ihdr.BuildTree_Arcene();
            Console.WriteLine(ihdr.ResultMessage);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            ihdr.BuildTree_Gisette();
            Console.WriteLine(ihdr.ResultMessage);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            GisetteParser parser = new GisetteParser(
                   @"D:\Dropbox\DP\datasets\Gisette\gisette_train.data",
                   @"D:\Dropbox\DP\datasets\Gisette\gisette_train.labels",
                   @"D:\Dropbox\DP\datasets\Gisette\gisette_valid.data",
                   @"D:\Dropbox\DP\datasets\Gisette\gisette_valid.labels");

            parser.ParseData();
            parser.ParseDataTest();

            ihdr = new IHDR();
            List<GisetteParserLib.Sample> arceneSamples = parser.Samples;
            foreach (var item in arceneSamples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
            foreach (var item in parser.SamplesTest)
            {
                ihdr.AddTestingSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            FacesParser parser = new FacesParser(
                    @"D:\Dropbox\DP\datasets\Faces\train\face\",
                    @"D:\Dropbox\DP\datasets\Faces\train\non-face\",
                    @"D:\Dropbox\DP\datasets\Faces\test\face\",
                    @"D:\Dropbox\DP\datasets\Faces\test\non-face\");

            parser.ParseData();
            parser.ParseDataTest();

            ihdr = new IHDR();
            List<FacesParserLib.Sample> mnistSamples = parser.Samples;
            foreach (var item in mnistSamples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
            foreach (var item in parser.SamplesTest)
            {
                ihdr.AddTestingSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            ihdr.BuildTree_Faces();
            Console.WriteLine(ihdr.ResultMessage);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            Faces2Parser parser = new Faces2Parser(
                    @"D:\Dropbox\DP\datasets\Faces2\");

            parser.ParseData();
            parser.ParseDataTest();

            ihdr = new IHDR();
            foreach (var item in parser.Samples)
            {
                ihdr.AddSample(item.GetAttributesArray(), (double)item.Label);
            }
            foreach (var item in parser.SamplesTest)
            {
                ihdr.AddTestingSample(item.GetAttributesArray(), (double)item.Label);
            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            Settings.SetSettings_Faces2();
            ihdr.BuildTree_Faces2();
            Console.WriteLine(ihdr.ResultMessage);
        }

        
    }
}
