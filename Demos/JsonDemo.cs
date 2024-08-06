using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Otus.Serialization
{
    internal enum Status
    {
        Open = 1,

        Closed = 2
    }

    public class JsonDemo
    {
        public static void Show()
        {
            var pages = new[] { new Page(1), new Page(2), new Page(3) };
            var books = new[] { new Book("Война и мир The Book") { Pages = pages } };

            var opt = new JsonSerializerOptions
            {
                WriteIndented = true,
                //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = true,
            };

            var library = new Library
            {
                Status = Status.Open,
                Name = "им. Ленина",
                Books = books
            };
            
            var s = JsonSerializer.Serialize(library, opt);
            File.WriteAllText("file.json", s);

            var ffff = File.ReadAllText("file.json");
            var json = (IDictionary<string, object>)JsonSerializer.Deserialize<ExpandoObject>(ffff);

            Console.WriteLine(json["name"]);
        }
    }

    public class MyConverter : JsonConverter<int>
    {
        public override int Read(
          ref Utf8JsonReader reader,
           System.Type typeToConvert,
            JsonSerializerOptions options)
        {
            var s = reader.GetString();

            Console.WriteLine(s);

            return int.Parse(s.Replace("fancy_number_", ""));
        }

        public override void Write(
          Utf8JsonWriter writer,
           int value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue($"fancy_number_{value}");                                                                                                    //JsonStringEnumConverter
        }
    }

    internal class Book
    {
        public string ShouldInclude;

        public Book(string name)
        {
            Name = name;
            ShouldInclude = "TRUE";
        }

        public Book()
        {
        }

        public string Name { get; set; }
        public IEnumerable<Page> Pages { get; set; }
    }

    internal class Library
    {
        [JsonPropertyName("BBBook_collection")]
        public IEnumerable<Book> Books { get; set; }

        public string Name { get; set; }
        public Status Status { get; set; }
    }

    internal class Page
    {
        public Page(int num)
        {
            Number = num;
        }

        public Page()
        {
        }

        [JsonConverter(typeof(MyConverter))]
        public int Number { get; set; }
    }

    // FooProperty
    // fooProperty
    // foo_property
}