using HotelServiceAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SQLDatabase;
using System.Threading.Tasks;

namespace HotelServiceAPI.Controllers
{
    public class HotelController : ApiController
    {
        private static Logger messageLogger;
        // GET: api/Hotel
        [HttpGet]
        [Route("api/Hotel")]
        public async Task<IEnumerable<ResponseHotelData>> GetAllHotels()
        {
            messageLogger = Logger.Instance;
            messageLogger.Log("GetAllHotels() API is invoked");
            List<HotelThirdPartyData> hotelsFromThirdApi=new List<HotelThirdPartyData>();
            List<HotelStaticData> staticHotelData = new List<HotelStaticData>();
            Task task1 = new Task(async () =>
              {
                  hotelsFromThirdApi = await ThirdPartyHotelProvider();
              });
            staticHotelData = StaticHotelDataProvider();
            task1.Start();
            await task1;
            List<ResponseHotelData> completeHotelData = new List<ResponseHotelData>();

            foreach (var hotels in hotelsFromThirdApi)
            {
                ResponseHotelData responseHotelData = new ResponseHotelData();
                responseHotelData.HotelId = hotels.HotelId;
                responseHotelData.HotelName = hotels.HotelName;
                responseHotelData.HotelAddress = hotels.HotelAddress;
                responseHotelData.AvailableNoOfRooms = hotels.AvailableNoOfRooms;
                var staticDataOfHotel = staticHotelData.Find(x => x.HotelId == hotels.HotelId);
                responseHotelData.ImageURL = HttpContext.Current.Server.MapPath("~") + staticDataOfHotel.ImageURL;
                responseHotelData.Amenities = staticDataOfHotel.Amenities;
                completeHotelData.Add(responseHotelData);
            }
            messageLogger.Log("GetAllHotels() API return list of Hotels");
            return completeHotelData;
        }

        private static List<HotelStaticData> StaticHotelDataProvider()
        {
            messageLogger = Logger.Instance;
            messageLogger.Log("StaticHotelDataProvider() Method is invoked");
            List<HotelStaticData> staticHotelData = new List<HotelStaticData>();
            var path = HttpContext.Current.Server.MapPath("~/HotelList.json");
            using (StreamReader streamReader = new StreamReader(path))
            {
                var readData = streamReader.ReadToEnd();
                staticHotelData = JsonConvert.DeserializeObject<List<HotelStaticData>>(readData);
            }
            messageLogger.Log("StaticHotelDataProvider() Method returned the list of static data");
            return staticHotelData;
        }

        private static async Task<List<HotelThirdPartyData>> ThirdPartyHotelProvider()
        {
            messageLogger = Logger.Instance;
            messageLogger.Log("ThirdPartyHotelProvider() Method is invoked");
            string url = "http://localhost:64729/HotelServiceOperations.svc/Hotels";
            WebClient webClient = new WebClient();
            var json = webClient.DownloadString(url);
            List<HotelThirdPartyData> hotelsFromThirdApi = JsonConvert.DeserializeObject<List<HotelThirdPartyData>>(json);
            messageLogger.Log("ThirdPartyHotelProvider() Method returned the list of Dynamic Hotel Data");
            return hotelsFromThirdApi;
        }

        // GET: api/Hotel/5
        [HttpGet]
        [Route("api/Hotel/{hotelId}")]
        public IEnumerable<Rooms> GetRoomsByHotelId(int hotelId)
        {
            messageLogger = Logger.Instance;
            messageLogger.Log("GetRoomsByHotelId() API is invoked");
            string url = "http://localhost:64729/HotelServiceOperations.svc/Rooms/"+hotelId;
            WebClient webClient = new WebClient();
            var json = webClient.DownloadString(url);
            List<Rooms> rooms = JsonConvert.DeserializeObject<List<Rooms>>(json);
            messageLogger.Log("GetRoomsByHotelId() API returned list of rooms for a hotel");
            return rooms;
        }

        // POST: api/Hotel
        [HttpPost]
        [Route("api/Hotel")]
        public void BookAHotelRoom([FromBody]BookingDetails bookingDetails)
        {
            messageLogger = Logger.Instance;
            messageLogger.Log("BookAHotelRoom() API is invoked");
            string url = "http://localhost:64729/HotelServiceOperations.svc/Rooms/" + bookingDetails.HotelId + "/" + bookingDetails.RoomId;
            HttpClient webClient = new HttpClient();
            webClient.BaseAddress = new Uri("http://localhost:64729/");
            RoomUpdate roomUpdate = new RoomUpdate() { HotelId=bookingDetails.HotelId, RoomId = bookingDetails.RoomId };
            var response = webClient.PutAsJsonAsync("HotelServiceOperations.svc/Rooms", roomUpdate).Result;

            SqlDB sqlDB = new SqlDB();
            sqlDB.InsertBookings(bookingDetails.RoomId, bookingDetails.HotelId, bookingDetails.RoomFare);
            messageLogger.Log("BookAHotelRoom() API saved Hotel Room Details");
        }
    }
}
