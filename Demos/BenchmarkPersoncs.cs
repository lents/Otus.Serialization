using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Runtime.Serialization.Json;

namespace Otus.Serialization.Demos
{
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public int Age { get; set; }

        [ProtoMember(3)]
        public string Email { get; set; }
    }

    public class SerializationBenchmarks
    {
        private Person person;
        private byte[] serializedData;

        [GlobalSetup]
        public void Setup()
        {
            person = new Person { Name = "Alice", Age = 30, Email = "alice@example.com" };
            serializedData = null;
        }

        [Benchmark]
        public void BinaryFormatter_Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, person);
                serializedData = memoryStream.ToArray();
            }
        }       

        [Benchmark]
        public void XmlSerializer_Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(Person));
                serializer.Serialize(memoryStream, person);
                serializedData = memoryStream.ToArray();
            }
        }

        [Benchmark]
        public void DataContractSerializer_Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(Person));
                serializer.WriteObject(memoryStream, person);
                serializedData = memoryStream.ToArray();
            }
        }      
        

        [Benchmark]
        public void ProtobufNet_Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memoryStream, person);
                serializedData = memoryStream.ToArray();
            }
        }       
    }
}