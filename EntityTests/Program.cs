using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

namespace EntityTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "test.json";

            // create our Star System:
            StarSystem ss = new StarSystem();
            ss.Name = "Test No 1";
            GameState.StarSystems.Add(ss);

            // Create an instance of the type and serialize it.
            TestEntity test1 = new TestEntity();
            test1.Name = "testing 1 2 3";

            ss.AddNewEntity(test1);  // must add it to the SS before it has components!!
            var ruins = new RuinsDB();
            ss.GetDataBlobList<List<RuinsDB>>(DataBlobIndex.RuinsDB)[test1.DataBlobsIndex] = ruins;

            SerializeItem(fileName, test1);
            TestEntity test2 = DeserializeItem(fileName, typeof(TestEntity)) as TestEntity;
        }

        public static void SerializeItem(string fileName, DataEntity entity)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, entity);
            }
        }

        public static DataEntity DeserializeItem(string fileName, Type entityType)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                DataEntity e = serializer.Deserialize(reader, entityType) as DataEntity;
                return e;
            }
        }
    }
}
