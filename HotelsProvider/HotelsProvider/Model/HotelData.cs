using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HotelsProvider.Model
{
    [DataContract]
    public class HotelData
    {
        [DataMember]
        public int HotelId;
        [DataMember]
        public string HotelName;
        [DataMember]
        public string HotelAddress;
        [DataMember]
        public int AvailableNoOfRooms;
    }
}