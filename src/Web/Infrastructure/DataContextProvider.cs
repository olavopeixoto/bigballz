using System.Data.Common;
using BigBallz.Models;
using StackExchange.Profiling;

namespace BigBallz.Infrastructure
{
    public class DataContextProvider
    {
        public BigBallzDataContext CreateContext()
        {
            var conn = new StackExchange.Profiling.Data.ProfiledDbConnection(GetConnection(), MiniProfiler.Current);
            return new BigBallzDataContext(conn);
        }

        private static DbConnection GetConnection()
        {
            return new BigBallzDataContext().Connection;
        }
    }
}