using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CassandraDatabase;
using HotelsProvider.Model;

namespace HotelsProvider
{
     public class HotelServiceOperations : IHotelService
    {
        public void UpdateOnRoomBook(RoomUpdate roomUpdate)
        {
            CassandraDB cassandra = new CassandraDB();
            cassandra.UpdateOnRoomBook(roomUpdate.HotelId, roomUpdate.RoomId);
        }

        List<Hotel> IHotelService.GetAllHotels()
        {
            CassandraDB cassandra = new CassandraDB();
            return cassandra.GetAllHotels();
        }

        List<Room> IHotelService.GetAllRoomsByHotelId(string hotelId)
        {
            CassandraDB cassandra = new CassandraDB();
            return cassandra.GetAllRoomsOfHotel(int.Parse(hotelId));
        }
    }
}
