using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelServiceAPI.Models
{
    public class BookingDetails
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public double RoomFare { get; set; }
    }
}