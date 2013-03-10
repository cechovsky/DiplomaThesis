using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntheticDataGenerator
{
    public class SyntheticDataGenerator
    {
        public List<Sample> samples;
        public List<Sample> samplesTest;
        double[] center1;
        double[] center2;
        double[] center3;
        double[] center1Y;
        double[] center2Y;
        double[] center3Y;
        double delta;
        double deltaY;

        Random r = new Random();

        public SyntheticDataGenerator(double delta, double deltaY)
        {
            this.samples = new List<Sample>();
            this.samplesTest = new List<Sample>();
            this.delta = delta;
            this.deltaY = deltaY;
            this.center1 = new double[] { 0, 0, 200 };
            this.center2 = new double[] { 0, 200, 0 };
            this.center3 = new double[] { 200, 0, 0 };
            this.center1Y = new double[] { 0, 0 };
            this.center2Y = new double[] { 0, 200 };
            this.center3Y = new double[] { 200, 0 };

        }

        //public void GenerateSyntheticData()
        //{
        //    for (int i = 0; i < 500; i++)
        //    {
        //        Sample s1 = new Sample();
        //        s1.Label = 1;
        //        s1.AddAttribute((center1[0] + (((r.NextDouble() * 2) - 1) * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s1.AddAttribute((center1[1] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s1.AddAttribute((center1[2] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s1.AddAttributeY((center1Y[0] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));
        //        s1.AddAttributeY((center1Y[1] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));

        //        Sample s2 = new Sample();
        //        s2.Label = 2;
        //        s2.AddAttribute((center2[0] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s2.AddAttribute((center2[1] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s2.AddAttribute((center2[2] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s2.AddAttributeY((center2Y[0] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));
        //        s2.AddAttributeY((center2Y[1] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));

        //        Sample s3 = new Sample();
        //        s3.Label = 3;

        //        s3.AddAttribute((center3[0] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s3.AddAttribute((center3[1] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s3.AddAttribute((center3[2] - this.delta) + ((r.NextDouble() * 2 * this.delta)) * (1 + (r.NextDouble() - 0.5)));
        //        s3.AddAttributeY((center3Y[0] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));
        //        s3.AddAttributeY((center3Y[1] - this.deltaY) + ((r.NextDouble() * 2 * this.deltaY)) * (1 + (r.NextDouble() - 0.5)));

        //        this.samples.Add(s1);
        //        this.samples.Add(s2);
        //        this.samples.Add(s3);
        //    }
        //}

        public void GenerateSyntheticData()
        {
            for (int i = 0; i < 500; i++)
            {
                this.samples.Add(this.GetRandomSample(center1, center1Y, this.delta, this.deltaY, 1));
                this.samples.Add(this.GetRandomSample(center2, center2Y, this.delta, this.deltaY, 2));
                this.samples.Add(this.GetRandomSample(center3, center3Y, this.delta, this.deltaY, 3));
            }
        }

        private Sample GetRandomSample(double[] center, double[] centerY, double deltaX, double deltaY, byte label)
        {
            double randomRadian = r.NextDouble() * 2 * Math.PI;
            double x = Math.Cos(randomRadian) * deltaX * r.NextDouble();
            double y = Math.Sin(randomRadian) * deltaX * r.NextDouble();
            double z = deltaX * ((r.NextDouble() * 2) - 1);

            Sample newSample = new Sample();
            newSample.AddAttribute(center[0] + x);
            newSample.AddAttribute(center[1] + y);
            newSample.AddAttribute(center[2] + z);

            double randomRadian2 = r.NextDouble() * 2 * Math.PI;
            double x2 = Math.Cos(randomRadian) * deltaY * r.NextDouble();
            double y2 = Math.Sin(randomRadian) * deltaY * r.NextDouble();

            newSample.AddAttributeY(centerY[0] + x2);
            newSample.AddAttributeY(centerY[1] + y2);

            newSample.Label = label;

            return newSample;            
        }

        public void GenerateSyntheticDataTest()
        {
            for (int i = 0; i < 100; i++)
            {
                this.samplesTest.Add(this.GetRandomSample(center1, center1Y, this.delta, this.deltaY, 1));
                this.samplesTest.Add(this.GetRandomSample(center2, center2Y, this.delta, this.deltaY, 2));
                this.samplesTest.Add(this.GetRandomSample(center3, center3Y, this.delta, this.deltaY, 3));
            }
        }
    }
}
