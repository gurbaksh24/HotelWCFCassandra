using CassandraDatabase;
using HotelsProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HotelsProvider
{
    [ServiceContract]
    interface IHotelService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Hotels", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<Hotel> GetAllHotels();
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Rooms/{hotelId}")]
        List<Room> GetAllRoomsByHotelId(string hotelId);
        [OperationContract]
        [WebInvoke(Method="PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Rooms")]
        void UpdateOnRoomBook(RoomUpdate roomUpdate);
    }
}
