using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Otus.Serialization
{
    public enum ShopType
    {
        Private = 1,
        Public = 2
    }

    [XmlRoot("ShopShop")]
    public class AutoShop
    {
        [XmlArray("MyBeautifulCars")]
        [XmlArrayItem("MyCar")]
        public Car[] Cars { get; set; }

        public ShopType ShopType { get; set; }

        [XmlAttribute]
        public int Version { get; set; }
    }

    [XmlRoot("MyCar1")]
    public class Car
    {
        [XmlAttribute("BarColor")]
        public string Color { get; set; }

        [XmlIgnore]
        public string Name { get; set; }
        [XmlElement("TestPrice")]
        public int Price { get; set; }
    }

    public class XmlDemo
    {
        public static void Show()
        {
            var car = new Car { Name = "LADA", Color = "Red", Price = 222 };
            var car1 = new Car { Name = "Ниссан", Color = "Blue", Price = 22352 };
            var shop = new AutoShop { Cars = new[] { car, car1 }, ShopType = ShopType.Public };

            var s = new XmlSerializer(typeof(AutoShop));

            using (var fs = new FileStream("demo.xml", FileMode.Create))
            {
                s.Serialize(fs, shop);
            }

            using (var fs = new FileStream("demo.xml", FileMode.Open))
            {
                var obj = (AutoShop)s.Deserialize(fs);
                Console.WriteLine(obj.Cars[1].Color);
            }
        }
    }
}