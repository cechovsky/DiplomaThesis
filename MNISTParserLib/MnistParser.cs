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
        private List<Sample> samples;

        public MnistParser(string samplesPath, string labelsPath)
        {
            this.samplesPath = samplesPath;
            this.labelsPath = labelsPath;
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

        public List<Sample> Samples
        {
            get
            {
                return samples;
            }
        }

        public void SaveSamplesToBmp(string path)
        {
            foreach (var item in samples)
            {
                item.SaveToBitmap(path);
            }
        }


    }
}
