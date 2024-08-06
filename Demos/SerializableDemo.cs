using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Otus.Serializer
{
    [Serializable]
    public class GameSave : ISerializable
    {
        public DateTime? SaveTimestamp;

        public GameSave()
        {
            SaveTimestamp = new DateTime(2020, 1, 1);
        }

        // Особый конструктор - при десериализации
        public GameSave(SerializationInfo info, StreamingContext context)
        {
            Console.WriteLine("Deserializing");
            SaveTimestamp = info.GetDateTime("saveTimestamp1");
        }

        public int A { get; set; }

        //   public int B{get;set;}

        //   public int C{get;set;}
        // Из ISerializable, вызывается при сериализации
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var now = DateTime.Now;
            if (now < new DateTime(2020, 01, 01))
            {
                throw new Exception("ERROR");
            }
            Console.WriteLine($"Custom Serializing now: {now}");

            info.AddValue("saveTimestamp1", now);

            // ...
        }

        public override string ToString()
        {
            return $"A: {A}, SaveTimestamp: {SaveTimestamp}";
        }
    }

    public class SerializableDemo
    {
        public static void Show()
        {
            var bf = new BinaryFormatter();

            var before = new GameSave();

            Console.WriteLine($"before: {before}");

            using (var fs = new FileStream("serDemo.bin", FileMode.Create))
            {
                bf.Serialize(fs, before);
            }

            Console.Write("Нажмите кнопку: ");
            Console.WriteLine();
            Console.ReadKey();

            using (var fs = new FileStream("serDemo.bin", FileMode.Open))
            {
                Console.WriteLine($"Время и дата: {DateTime.Now}");
                var after = (GameSave)bf.Deserialize(fs);

                Console.WriteLine($"after: {after}");
            }
        }
    }
}