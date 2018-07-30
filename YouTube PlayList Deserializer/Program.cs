using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using YouTube_PlayList_Deserializer.Models;

namespace YouTube_PlayList_Deserializer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileName = @"Wedding Songs.json";
                var json = System.IO.File.ReadAllText(fileName);

                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(PlayListContainer));
                    PlayListContainer bsObj2 = (PlayListContainer) deserializer.ReadObject(ms);
                }

                var playListContainer = new PlayListContainer();
                playListContainer.Videos = new List<Video>()
                {
                    new Video()
                    {
                        ContentDetails = new contentDetails()
                        {
                            videoId = "1",
                            videoPublishedAt = DateTime.Now
                        }
                    }
                };

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PlayListContainer));
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, playListContainer);
                StreamReader sr = new StreamReader(stream);

                // "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
                string output = sr.ReadToEnd();

                sr.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            Console.Read();
        }
    }
}
