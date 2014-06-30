using System;
using System.IO;
using BigBallz.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigBall.Tests
{
    [TestClass]
    public class Install
    {
        [TestMethod]
        public void CreateDatabase()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bigballz.mdf");
            var db = new BigBallzDataContext(fileName);
            db.CreateDatabase();
        }
    }
}
