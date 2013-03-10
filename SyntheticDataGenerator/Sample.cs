using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntheticDataGenerator
{
    public class Sample
    {
        public List<double> Attributes { get; set; }
        public List<double> AttributesY { get; set; }
        public byte Label { get; set; }
        public int Id { get; set; }

        public Sample()
        {
            Attributes = new List<double>();
            AttributesY = new List<double>();
            Label = 0;
        }

        public Sample(byte label, int id)
        {
            Attributes = new List<double>();
            AttributesY = new List<double>();
            Label = label;
            Id = id;
        }

        public void AddAttribute(double attribute)
        {
            if (Attributes != null)
            {
                Attributes.Add(attribute);
            }
        }

        public void AddAttributeY(double attribute)
        {
            if (AttributesY != null)
            {
                AttributesY.Add(attribute);
            }
        }
    }
}
