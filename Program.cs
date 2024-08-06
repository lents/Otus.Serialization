using System;
using BenchmarkDotNet.Running;
using Otus.Serialization.Demos;
using Otus.Serializer;
using Otus.Serializer.Demos;

namespace Otus.Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            //AttributeDemo.Show();
            //JsonDemo.Show();
            //SerializableDemo.Show();
            //XmlDemo.Show();
            ProtoDemo.Show();

            Console.WriteLine("Hello World!");
           // var summary = BenchmarkRunner.Run<TimerDemo>();
            var summary = BenchmarkRunner.Run<SerializationBenchmarks>();
            Console.WriteLine("Good bye!");
        }
    }
}
