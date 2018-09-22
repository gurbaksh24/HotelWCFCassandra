using CQLDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelServiceAPI.Models
{
    public class Logger
    {
        private static Logger instance;

        private Logger()
        {

        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }
        public void Log(string logMessage)
        {
            CqlDB cqlDB = new CqlDB();
            cqlDB.InsertLogs(logMessage);
        }
    }
}