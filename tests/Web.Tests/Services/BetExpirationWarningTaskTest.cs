using System;
using System.Collections.Generic;
using BigBallz.Infrastructure;
using BigBallz.Models;
using BigBallz.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Match = BigBallz.Models.Match;

namespace BigBall.Tests.Services
{
    [TestClass]
    public class BetExpirationWarningTaskTest
    {
        private readonly Mock<IMailService> _mailMock = new Mock<IMailService>();
        private readonly BetExpirationWarningTask _task;

        public BetExpirationWarningTaskTest()
        {
            var provider = new DataContextProvider();

            _task = new BetExpirationWarningTask(_mailMock.Object, DateTime.Now, TimeSpan.FromSeconds(1), provider);
        }

        [TestMethod]
        public void Run_All()
        {
            _mailMock.Setup(x => x.SendBetWarning(It.IsAny<User>(), It.IsAny<IList<Match>>()));
            _task.Run();

            _mailMock.Verify(x => x.SendBetWarning(It.IsAny<User>(), It.IsAny<IList<Match>>()), Times.AtLeastOnce);
        }
    }
}
