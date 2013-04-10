using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacesParserLib
{
    public class FacesParser
    {
        private string trainFace;
        private string trainNonFace;
        private string testFace;
        private string testNonFaces;
        private List<Sample> samples;
        private List<Sample> samplesTest;

        public FacesParser(string trainFace, string trainNonFace, string testFace, string testNonFaces)
        {
            this.trainFace = trainFace;
            this.trainNonFace = trainNonFace;
            this.testFace = testFace;
            this.testNonFaces = testNonFaces;
        }

        public void ParseData()
        {
            #region face samples

            var newSamples = new List<Sample>();

            int i = 0;
            var filenames = Directory.GetFiles(trainFace).Select(f => Path.GetFileName(f));
            foreach (var filename in filenames)
            {
                i++;
                using (BinaryReader samplesReader = new BinaryReader(File.Open(trainFace + filename, FileMode.Open)))
                {
                    samplesReader.ReadBytes(13);
                    //label 1 facedata
                    Sample sample = new Sample(1, i);

                    int attributesCount = 19 * 19;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        byte newByte = samplesReader.ReadByte();
                        sample.AddAttribute(newByte);
                    }

                    newSamples.Add(sample);
                }
            }

            #endregion

            // load non faces
            var filenames2 = Directory.GetFiles(trainNonFace).Select(f => Path.GetFileName(f));

            foreach (var filename in filenames2)
            {
                i++;
                using (BinaryReader samplesReader = new BinaryReader(File.Open(trainNonFace + filename, FileMode.Open)))
                {
                    samplesReader.ReadBytes(13);
                    //label 1 facedata
                    Sample sample = new Sample(0, i);

                    int attributesCount = 19 * 19;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        byte newByte = samplesReader.ReadByte();
                        sample.AddAttribute(newByte);
                    }

                    newSamples.Add(sample);
                }
            }

            this.samples = newSamples.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public void ParseDataTest()
        {
            #region face samples

            var newSamples = new List<Sample>();

            int i = 0;
            var filenames = Directory.GetFiles(testFace).Select(f => Path.GetFileName(f));
            foreach (var filename in filenames)
            {
                i++;
                using (BinaryReader samplesReader = new BinaryReader(File.Open(testFace + filename, FileMode.Open)))
                {
                    samplesReader.ReadBytes(13);
                    //label 1 facedata
                    Sample sample = new Sample(1, i);

                    int attributesCount = 19 * 19;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        byte newByte = samplesReader.ReadByte();
                        sample.AddAttribute(newByte);
                    }

                    newSamples.Add(sample);
                }
            }

            #endregion

            // load non faces
            var filenames2 = Directory.GetFiles(testNonFaces).Select(f => Path.GetFileName(f)).Take(472);

            foreach (var filename in filenames2)
            {
                i++;
                using (BinaryReader samplesReader = new BinaryReader(File.Open(testNonFaces + filename, FileMode.Open)))
                {
                    samplesReader.ReadBytes(13);
                    //label 1 facedata
                    Sample sample = new Sample(0, i);

                    int attributesCount = 19 * 19;

                    for (int j = 0; j < attributesCount; j++)
                    {
                        byte newByte = samplesReader.ReadByte();
                        sample.AddAttribute(newByte);
                    }

                    newSamples.Add(sample);
                }
            }

            this.samplesTest = newSamples.OrderBy(a => Guid.NewGuid()).ToList();
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
