using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faces2ParserLib
{
    public class Sample
    {
        public List<byte> Attributes { get; set; }
        public byte Label { get; set; }
        public int Id { get; set; }

        public Sample()
        {
            Attributes = new List<byte>();
            Label = 0;
        }

        public Sample(byte label, int id)
        {
            Attributes = new List<byte>();
            Label = label;
            Id = id;
        }

        public void AddAttribute(byte attribute)
        {
            if (Attributes != null)
            {
                Attributes.Add(attribute);
            }
        }

        public Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb(Attributes[i * 28 + j], Attributes[i * 28 + j], Attributes[i * 28 + j]));
                }
            }

            return bitmap;
        }

        public void SaveToBitmap(string locationPath)
        {
            Bitmap bitmap = new Bitmap(54, 72);

            for (int i = 0; i < 54; i++)
            {
                for (int j = 0; j < 72; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(Attributes[i * 72 + j], Attributes[i * 72 + j], Attributes[i * 72 + j]));
                }
            }

            bitmap.Save(locationPath + @"\sample_" + this.Id + "_" + this.Label + ".bmp");
        }

        public double[] GetAttributesArray()
        {
            double[] result = Attributes.Select(i => Convert.ToDouble(i)).ToArray<double>();
            return result;
        }
    }
}
