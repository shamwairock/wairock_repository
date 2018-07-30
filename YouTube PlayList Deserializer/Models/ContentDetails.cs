using System;
using System.Runtime.Serialization;

namespace YouTube_PlayList_Deserializer.Models
{
    [DataContract]
    public class contentDetails
    {
        [DataMember]
        public string videoId { get; set; }

        [DataMember]
        public DateTime videoPublishedAt { get; set; }
    }
}
