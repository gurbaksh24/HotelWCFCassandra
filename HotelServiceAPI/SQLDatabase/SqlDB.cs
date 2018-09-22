using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDatabase
{
    public class SqlDB
    {
        public bool InsertBookings(int roomId, int hotelId, double roomFare)
        {
            try
            {
                HotelBookingEntities hotelBookingEntities = new HotelBookingEntities();
                BookedHotelRoom bookedHotelRoom = new BookedHotelRoom();
                bookedHotelRoom.RoomId = roomId;
                bookedHotelRoom.HotelId = hotelId;
                bookedHotelRoom.RoomFare = (decimal)roomFare;
                hotelBookingEntities.BookedHotelRooms.Add(bookedHotelRoom);
                hotelBookingEntities.SaveChanges();
                return true;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }
    }
}
