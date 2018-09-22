using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQLDatabase
{
    public class CqlDB
    {
        public ISession GetSession()
        {
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("HotelLogging");
            return session;
        }
        public void InsertLogs(string logMessage)
        {
            try
            {
                ISession session = GetSession();
                string query = "Insert into \"Logging\"(\"LogTime\",\"LogMessage\") values(?,?)";
                PreparedStatement preparedStatement = session.Prepare(query);
                BoundStatement boundStatement = preparedStatement.Bind(DateTime.Now,logMessage);
                RowSet row = session.Execute(boundStatement);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
