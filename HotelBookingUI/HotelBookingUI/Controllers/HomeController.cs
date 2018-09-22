using HotelBookingUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
namespace HotelBookingUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllHotels()
        {
            string url = "http://localhost:49647/api/Hotel";
            WebClient webClient = new WebClient();
            var json = webClient.DownloadString(url);
            List<Hotel> hotelList = JsonConvert.DeserializeObject <List<Hotel>>(json);
            TempData["HotelList"] = hotelList;
            return RedirectToAction("Index");
        }
        public ActionResult GetRoomsByHotelId()
        {
            string hotelId = Request.Params["hotelId"];
            string url = "http://localhost:49647/api/Hotel/"+hotelId;
            WebClient webClient = new WebClient();
            var json = webClient.DownloadString(url);
            List<Room> roomList = JsonConvert.DeserializeObject<List<Room>>(json);
            TempData["RoomList"] = roomList;
            return RedirectToAction("Index");
        }
        public ActionResult BookRoom()
        {
            string[] roomDetails = Request.Params["roomId"].Split(':');
            Room room = new Room();
            room.HotelId = int.Parse(roomDetails[0]);
            room.RoomId = int.Parse(roomDetails[1]);
            room.RoomFare = double.Parse(roomDetails[2]);
            HttpClient webClient = new HttpClient();
            webClient.BaseAddress = new Uri("http://localhost:49647/");
            var response = webClient.PostAsJsonAsync("api/Hotel", room).Result;
            return RedirectToAction("Index");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}