using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faces2ParserLib
{
    public class Faces2Parser
    {
        private string trainFace;
        private string trainNonFace;
        private string testFace;
        private string testNonFaces;
        private List<Sample> samples;
        private List<Sample> samplesTest;

        public Faces2Parser(string trainFace)
        {
            this.trainFace = trainFace;            
        }

        public void ParseData()
        {
            var newSamples = new List<Sample>();

            int i = 0;
            var filenames = Directory.GetFiles(trainFace).Select(f => Path.GetFileName(f)).ToList();
            foreach (var filename in filenames)
            {
                i++;
                var image = new Bitmap(trainFace + filename);

                Sample newSample = new Sample();
                newSample.Id = i;
                switch (i % 7)
                {
                    case 1:
                        newSample.Label = 1;
                        break;
                    case 2:
                        newSample.Label = 2;
                        break;
                    case 3:
                        newSample.Label = 3;
                        break;
                    case 4:
                        newSample.Label = 4;
                        break;
                    case 5:
                        newSample.Label = 5;
                        break;
                    case 6:
                        newSample.Label = 6;
                        break;
                    case 0:
                        newSample.Label = 7;
                        break;
                    default:
                        break;
                }

                for (int ii = 0; ii < image.Width; ii++)
                {
                    for (int jj = 0; jj < image.Height; jj++)
                    {
                        Color color = image.GetPixel(ii,jj);
                        newSample.AddAttribute((byte)((color.R * .3) + (color.G * .59) + (color.B * .11)));
                    }
                }

                newSamples.Add(newSample);
            }

            this.samples = new List<Sample>();
            Random random = new Random();
            int id = 0;
            for (int r = 0; r < 100; r++)
            {
                foreach (var item in newSamples)
                {
                    id++;
                    Sample finalSample = new Sample(item.Label, id);
                    foreach (var item2 in item.Attributes)
	                {
		                int diff = (int)item2 - (int)(random.NextDouble() * 20);
                        if(diff < 0) diff = 0;
                        finalSample.AddAttribute(BitConverter.GetBytes(diff)[0]);        
	                }

                    this.samples.Add(finalSample);                                        
                }
            }

            this.samples = this.samples.OrderBy(a => Guid.NewGuid()).ToList();

        }

        public void ParseDataTest()
        {
            this.samplesTest = new List<Sample>();

            int i = 0;
            var filenames = Directory.GetFiles(trainFace).Select(f => Path.GetFileName(f)).ToList();
            foreach (var filename in filenames)
            {
                i++;
                var image = new Bitmap(trainFace + filename);

                Sample newSample = new Sample();
                newSample.Id = i;
                switch (i % 7)
                {
                    case 1:
                        newSample.Label = 1;
                        break;
                    case 2:
                        newSample.Label = 2;
                        break;
                    case 3:
                        newSample.Label = 3;
                        break;
                    case 4:
                        newSample.Label = 4;
                        break;
                    case 5:
                        newSample.Label = 5;
                        break;
                    case 6:
                        newSample.Label = 6;
                        break;
                    case 0:
                        newSample.Label = 7;
                        break;
                    default:
                        break;
                }

                for (int ii = 0; ii < image.Width; ii++)
                {
                    for (int jj = 0; jj < image.Height; jj++)
                    {
                        Color color = image.GetPixel(ii,jj);
                        newSample.AddAttribute((byte)((color.R * .3) + (color.G * .59) + (color.B * .11)));
                    }
                }

                this.samplesTest.Add(newSample);

            }

            this.samplesTest = this.samplesTest.OrderBy(a => Guid.NewGuid()).ToList();
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
