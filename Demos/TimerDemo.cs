using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Otus.Serialization
{

    [SimpleJob(RuntimeMoniker.Net50, 10, 10, 10, 3)]
    public class TimerDemo
    {
        private TestFile _testFile;

        public void Show()
        {
            _testFile = new TestFile();
            var r = new Random();

            for (var i = 0; i < 100000; i++)
            {
                _testFile.Doubles.Add(r.NextDouble() * 10000);
            }

            Binary();
            Json();
            Xml();
        }

        [Benchmark]
        public void Binary()
        {
            var tf = _testFile;
            var stopwatch = new Stopwatch();
            var bf = new BinaryFormatter();
            stopwatch.Start();

            using (var fs = new FileStream("bin.bin", FileMode.Create))
            {
                bf.Serialize(fs, tf);
            }

            stopwatch.Stop();

            // Console.WriteLine($"bin ser: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Reset();
            stopwatch.Start();
            using (var fs = new FileStream("bin.bin", FileMode.Open))
            {
                var after = (TestFile)bf.Deserialize(fs);

                // Console.WriteLine($"bin deser: {stopwatch.ElapsedMilliseconds}");
            }

            stopwatch.Stop();
        }

        [Benchmark]
        public void Json()
        {
            var tf = _testFile;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var json = JsonSerializer.Serialize(tf, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("json.json", json);

            stopwatch.Stop();

            // Console.WriteLine($"json ser: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Reset();
            stopwatch.Start();

            var after = JsonSerializer.Deserialize<TestFile>(File.ReadAllText("json.json"));
            stopwatch.Stop();
            // Console.WriteLine($"json deser: {stopwatch.ElapsedMilliseconds}");
        }

        [Benchmark]
        public void Xml()
        {
            var tf = _testFile;

            var stopwatch = new Stopwatch();
            var bf = new XmlSerializer(typeof(TestFile));
            stopwatch.Start();

            using (var fs = new FileStream("xml.xml", FileMode.Create))
            {
                bf.Serialize(fs, tf);
            }

            stopwatch.Stop();

            // Console.WriteLine($"xml ser: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Reset();
            stopwatch.Start();
            using (var fs = new FileStream("xml.xml", FileMode.Open))
            {
                var after = (TestFile)bf.Deserialize(fs);
                stopwatch.Stop();
                // Console.WriteLine($"xml deser: {stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}