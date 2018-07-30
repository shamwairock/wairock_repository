using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YouTube_PlayList_Deserializer.Models
{
    [DataContract]
    public class PlayListContainer
    {
        [DataMember]
        public IList<Video> Videos { get; set; }
    }
}
