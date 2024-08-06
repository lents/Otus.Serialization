using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Otus.Serializer.Demos
{
    public class ProtoDemo
    {
        public static void Show()
        {
            using (var fs = new FileStream("ff.bin", FileMode.Create))
            {
                var bw = new BinaryWriter(fs);

                bw.Write(124);
                bw.Write(false);
                bw.Write("!!");
            }

            using (var fs = new FileStream("ff.bin", FileMode.Open))
            {
                var bw = new BinaryReader(fs);

                var i = bw.ReadInt32();
                var b = bw.ReadBoolean();
                var s = bw.ReadString();
                Console.WriteLine($"i = {i} b={b} s={s}");
            }           
        }
    }

    [Serializable]
    [ProtoContract]
    public class TestMe
    {
        public TestMe()
        {
            Doubles = new List<double>();
        }

        [ProtoMember(1)]
        public List<double> Doubles { get; set; }

        [ProtoMember(2)]
        public double Foo { get; set; }
    }
}