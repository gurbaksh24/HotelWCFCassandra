using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelServiceAPI.Models
{
    public class ResponseHotelData
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public int AvailableNoOfRooms { get; set; }
        public string ImageURL { get; set; }
        public string Amenities { get; set; }
    }
}