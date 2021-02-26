using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FormatFinderCore
{
    public static class SaveHelper
    {
        public static void SaveAs(string path, SaveOption saveOption, object value, Type objType)
        {
            switch (saveOption)
            {
                case SaveOption.XML:
                    SaveXML(path, value);
                    break;
                case SaveOption.CSV:
                    SaveCSV(path, value, objType);
                    break;
                case SaveOption.TXT:
                    SaveCSV(path, value, objType);
                    break;
                case SaveOption.JSON:
                    SaveJSON(path, value);
                    break;
                default:
                    break;
            }
        }
        private static void SaveXML(string path, object value)
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(typeof(PagesWithFormatCollection));

            var fileName = path + ".xml";
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(stream, value);
            }
#if DEBUG
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                var check = (PagesWithFormatCollection)serializer.Deserialize(stream);
            }
#endif
        }
        private static void SaveJSON(string path, object value)
        {
            JsonSerializer serializer = new JsonSerializer();
            var fileName = path + ".json";
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                    serializer.Serialize(writer, value);
            }
#if DEBUG
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    using (JsonTextReader tr = new JsonTextReader(reader))
                    {
                        var check = (PagesWithFormatCollection)serializer.Deserialize(tr);
                    }
                }
            }
#endif
        }
        private static void SaveCSV(string path, object value, Type objType)
        {
            var fileName = path + ".csv";
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(string.Join(";", objType.GetProperties().Select(s => s.Name)));
                    if (value is IEnumerable<object> enumerable)

                        foreach (var item in enumerable)
                        {
                            var text = string.Join(";", item.GetType().GetProperties().Select(s => s.GetValue(item)));
                            writer.WriteLine(text);
                        }
                    else
                        foreach (var prop in objType.GetProperties())
                        {
                            var text = prop.GetValue(value);
                            writer.Write(text);
                        }
                }
            }
        }
    }
}
