using BigBallz.Models;
using BigBallz.Services.L2S;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigBall.Tests.Services
{
    [TestClass]
    public class BigBallzServiceTest
    {
        readonly BigBallzService _service = new BigBallzService(new BigBallzDataContext());
        
        [TestMethod]
        public void PlacarExato()
        {
            Assert.IsTrue(_service.PlacarExato(1, 0, 1, 0));
            Assert.IsTrue(_service.PlacarExato(1, 1, 1, 1));
            Assert.IsTrue(_service.PlacarExato(1, 2, 1, 2));
            
            Assert.IsFalse(_service.PlacarExato(2, 2, 1, 2));
            Assert.IsFalse(_service.PlacarExato(1, 1, 2, 2));
            Assert.IsFalse(_service.PlacarExato(2, 3, 2, 2));
        }

        [TestMethod]
        public void PlacarParcial()
        {
            Assert.IsTrue(_service.PlacarParcial(2, 0, 1, 0));
            Assert.IsTrue(_service.PlacarParcial(1, 3, 0, 3));

            Assert.IsFalse(_service.PlacarParcial(4, 1, 3, 2));
            Assert.IsFalse(_service.PlacarParcial(5, 1, 0, 2));
        }

        [TestMethod]
        public void Resultado()
        {
            Assert.IsTrue(_service.Resultado(2, 0, 3, 1));
            Assert.IsTrue(_service.Resultado(1, 3, 0, 1));

            Assert.IsFalse(_service.Resultado(4, 1, 2, 3));
            Assert.IsFalse(_service.Resultado(5, 1, 5, 6));
        }

        [TestMethod]
        public void Standing()
        {
            var standing = _service.GetStandings();
            Assert.IsNotNull(standing);
        }
    }
}
