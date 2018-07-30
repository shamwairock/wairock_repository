using System.Runtime.Serialization;

namespace YouTube_PlayList_Deserializer.Models
{
    [DataContract]
    public class Video
    {
        [DataMember]
        public contentDetails ContentDetails { get; set; }
    }
}
