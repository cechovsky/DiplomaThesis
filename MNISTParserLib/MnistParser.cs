using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNISTParserLib
{
    public class MnistParser
    {
        private string samplesPath;
        private string labelsPath;
        private string samplesPathTest;
        private string labelsPathTest;
        private List<Sample> samples;
        private List<Sample> samplesTest;

        public MnistParser(string samplesPath, string labelsPath, string samplesPathTest, string labelsPathTest)
        {
            this.samplesPath = samplesPath;
            this.labelsPath = labelsPath;
            this.samplesPathTest = samplesPathTest;
            this.labelsPathTest = labelsPathTest;
        }

        public void ParseData(int count)
        {
            samples = new List<Sample>();

            try
            {
                BinaryReader samplesReader = new BinaryReader(File.Open(samplesPath, FileMode.Open));
                BinaryReader labelsReader = new BinaryReader(File.Open(labelsPath, FileMode.Open));

                // read head of samples file
                samplesReader.ReadBytes(16);
                // read head of labels file 
                labelsReader.ReadBytes(8);

                for (int i = 0; i < count; i++)
                {
                    Sample sample = new Sample(labelsReader.ReadByte(), i);

                    int attributesCount = 28 * 28;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        sample.AddAttribute(samplesReader.ReadByte());
                    }

                    samples.Add(sample);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public void ParseDataTest(int count)
        {
            samplesTest = new List<Sample>();

            try
            {
                BinaryReader samplesReader = new BinaryReader(File.Open(samplesPathTest, FileMode.Open));
                BinaryReader labelsReader = new BinaryReader(File.Open(labelsPathTest, FileMode.Open));

                // read head of samples file
                samplesReader.ReadBytes(16);
                // read head of labels file 
                labelsReader.ReadBytes(8);

                for (int i = 0; i < count; i++)
                {
                    Sample sample = new Sample(labelsReader.ReadByte(), i);

                    int attributesCount = 28 * 28;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        sample.AddAttribute(samplesReader.ReadByte());
                    }

                    samplesTest.Add(sample);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public List<Sample> Samples
        {
            get
            {
                return samples;
            }
        }

        public List<Sample> SamplesTest
        {
            get
            {
                return samplesTest;
            }
        }

        public void SaveSamplesToBmp(string path)
        {
            foreach (var item in samples)
            {
                item.SaveToBitmap(path);
            }
        }

        public void SaveTestSamplesToBmp(string path)
        {
            foreach (var item in this.samplesTest)
            {
                item.SaveToBitmap(path);
            }
        }


    }
}
