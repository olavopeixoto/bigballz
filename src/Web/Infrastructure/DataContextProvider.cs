using System;
using System.Data.Common;
using System.IO;
using System.Web;
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
            if (HttpContext.Current==null || HttpContext.Current.IsDebuggingEnabled)
            {
                var context = new BigBallzDataContext();
                if (!context.DatabaseExists())
                {
                    context.Dispose();

                    var path = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "bigballz.mdf");

                    var dbContext = new BigBallzDataContext(path);
                    if (!dbContext.DatabaseExists())
                        dbContext.CreateDatabase();

                    return dbContext.Connection;
                }

                return context.Connection;
            }
            
            return new BigBallzDataContext().Connection;
        }
    }
}