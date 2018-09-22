using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBookingUI.Models
{
    public class Room
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public double RoomFare { get; set; }
        public string BookingStatus { get; set; }
    }
}