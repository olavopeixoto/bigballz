using BigBallz.Models;
using BigBallz.Services.L2S;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigBall.Tests.Services
{
    [TestClass]
    public class MailServiceTest
    {
        [TestMethod]
        public void SendEmail()
        {
            var mailService = new MailService();
            //var mockUser = new Mock<User>(MockBehavior.Relaxed);
            //mockUser.Expect(x => x.UserName).Returns("olavopeixoto");
            //mockUser.Expect(x => x.EmailAddress).Returns("olavopeixoto@gmail.com");
            //mockUser.Expect(x => x.EmailAddressVerified).Returns(false);
            //mailService.SendRegistration(mockUser.Object, "http://www.bigballz.com.br/pagamento", "http://www.bigballz.com.br/activate/fdoewnoqwdn==");

            mailService.SendMail("Olavo", "djbarney@gmail.com", "teste", "msg de teste");
        }

        [TestMethod]
        public void EmailRegex()
        {
            var attr = new EmailAttribute();
            Assert.IsTrue(attr.IsValid("itavares@casaevideo.com.br"));
            Assert.IsTrue(attr.IsValid("olavopeixoto@gmail.com"));
            Assert.IsTrue(attr.IsValid("teste_de_email.complexo@estrangeiro.co.uk"));
        }
    }
}
