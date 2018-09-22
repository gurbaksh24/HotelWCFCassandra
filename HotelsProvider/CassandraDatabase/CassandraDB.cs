using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
namespace CassandraDatabase
{
    public class CassandraDB
    {
        public ISession GetSession()
        {
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("Hotels");
            return session;
        }
        public List<Hotel> GetAllHotels()
        {
            try
            {
                ISession session = GetSession();
                string query = "Select * from \"HotelData\"";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind();
                RowSet row = session.Execute(boundStatement);
                List<Hotel> hotelList = new List<Hotel>();
                foreach (Row data in row)
                {
                    hotelList.Add(new Hotel()
                    {
                        HotelId = Convert.ToInt32(data["HotelId"]),
                        HotelName = data["HotelName"].ToString(),
                        HotelAddress = data["HotelAddress"].ToString(),
                        AvailableNoOfRooms = Convert.ToInt32(data["AvailableNoOfRooms"])
                    });
                }
                return hotelList;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return null;
            }
        }
        public List<Room> GetAllRoomsOfHotel(int hotelId)
        {
            try
            {
                ISession session = GetSession();
                string query = "Select * from \"Rooms\" where \"HotelId\"=?";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind(hotelId);
                RowSet row = session.Execute(boundStatement);
                List<Room> roomList = new List<Room>();
                foreach (Row data in row)
                {
                    roomList.Add(new Room()
                    {
                        HotelId = Convert.ToInt32(data["HotelId"]),
                        RoomId = Convert.ToInt32(data["RoomId"]),
                        RoomType = data["RoomType"].ToString(),
                        BookingStatus = data["BookingStatus"].ToString(),
                        RoomFare = Convert.ToDouble(data["RoomFare"])
                    });
                }
                return roomList;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return null;
            }
        }
        public void UpdateOnRoomBook(int hotelId, int roomId)
        {
            try
            {
                ISession session = GetSession();
                string query = "Update \"Rooms\" set \"BookingStatus\"=? where \"HotelId\"=? and \"RoomId\"=?";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind("Booked",hotelId,roomId);
                session.Execute(boundStatement);
                UpdateAvailableRooms(hotelId);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
        public void UpdateAvailableRooms(int hotelId)
        {
            try
            {
                int availableRooms = GetAvailableRooms(hotelId);
                ISession session = GetSession();
                string query = "Update \"HotelData\" set \"AvailableNoOfRooms\"=? where \"HotelId\"=?";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind(availableRooms, hotelId);
                session.Execute(boundStatement);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
        public int GetAvailableRooms(int hotelId)
        {
            try
            {
                ISession session = GetSession();
                string query = "Select * from \"HotelData\" where \"HotelId\"=?";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind(hotelId);
                Row row = session.Execute(boundStatement).First();
                return int.Parse(row["AvailableNoOfRooms"].ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return -1;
            }
        }
    }
}
