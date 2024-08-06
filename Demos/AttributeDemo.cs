using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Otus.Serialization
{
    public class AttributeDemo
    {
        public static void Show()
        {
            var bf = new BinaryFormatter();

            var before = new Bar(1, 3);

            Console.WriteLine($"before: {before}");

            using (var fs = new FileStream("attributeDemo.bin", FileMode.Create))
            {
                bf.Serialize(fs, before);
            }

            Console.ReadKey();
            using (var fs = new FileStream("attributeDemo.bin", FileMode.Open))
            {
                var after = (Bar)bf.Deserialize(fs);

                Console.WriteLine($"after: {after}");
            }
        }
    }

    [Serializable]
    public class Bar : Foo
    {
        public Bar(int a, int b) : base(a, b)
        { }
    }

    [Serializable]
    public class Foo
    {
        public int A;

        public int B;

        public Foo(int a, int b)
        {
            A = a;
            B = b;
        }

        // Не сериализуется
        public override string ToString()
        {
            return $"{{ A: {A}, B: {B}}}";
        }
    }
}