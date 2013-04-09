using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GisetteParserLib
{
    public class GisetteParser
    {
        private string samplesPath;
        private string labelsPath;
        private string samplesPathTest;
        private string labelsPathTest;
        private List<Sample> samples;
        private List<Sample> samplesTest;

        public GisetteParser(string samplesPath, string labelsPath, string samplesPathTest, string labelsPathTest)
        {
            this.samplesPath = samplesPath;
            this.labelsPath = labelsPath;
            this.samplesPathTest = samplesPathTest;
            this.labelsPathTest = labelsPathTest;
        }

        public void ParseData()
        {
            samples = new List<Sample>();
            
            string[] dataLines = File.ReadAllLines(this.samplesPath);
            string[] labelLines = File.ReadAllLines(this.labelsPath);

            if (dataLines.Length != labelLines.Length) throw new InvalidOperationException("Not the same count of rows in data samples and sample labels.");

            int i = 0;

            foreach (string line in dataLines)
            {
                List<string> attributes = line.Split(' ').ToList();

                Sample newSample = new Sample();
                int label = int.Parse(labelLines[i]);
                newSample.Label = label == -1 ? (byte)0 : (byte)1;
                newSample.Id = i + 1;

                foreach (var item in attributes)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        newSample.AddAttribute(int.Parse(item));
                    }
                }

                this.samples.Add(newSample);
                i++;
            }
        }

        public void ParseDataTest()
        {
            samplesTest = new List<Sample>();

            string[] dataLines = File.ReadAllLines(this.samplesPathTest);
            string[] labelLines = File.ReadAllLines(this.labelsPathTest);

            if (dataLines.Length != labelLines.Length) throw new InvalidOperationException("Not the same count of rows in data samples and sample labels.");

            int i = 0;
            foreach (string line in dataLines)
            {
                List<string> attributes = line.Split(' ').ToList();

                Sample newSample = new Sample();
                int label = int.Parse(labelLines[i]);
                newSample.Label = label == -1 ? (byte)0 : (byte)1;
                newSample.Id = i + 1;

                foreach (var item in attributes)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        newSample.AddAttribute(int.Parse(item));
                    }
                }

                this.SamplesTest.Add(newSample);
                i++;
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
