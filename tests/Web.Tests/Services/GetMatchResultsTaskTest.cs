using System;
using System.Threading.Tasks;
using BigBallz.Core;
using BigBallz.Core.Caching;
using BigBallz.Core.Social;
using BigBallz.Infrastructure;
using BigBallz.Services;
using BigBallz.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BigBall.Tests.Services
{
    [TestClass]
    public class GetMatchResultsTaskTest
    {
        private readonly GetMatchResultsTask _task;

        public GetMatchResultsTaskTest()
        {
            var provider = new DataContextProvider();
            var cache = new Mock<ICache>();

            _task = new GetMatchResultsTask(provider, cache.Object);
        }

        [TestMethod]
        public async Task Run()
        {
            await _task.RunAsync();
        }

        [TestMethod]
        public void Tweet()
        {
            Twitter.PostTweet("twitter@bigballz.com.br", "bb2018#$", "Já começou a segunda rodada do bolão, esse ano está ainda mais emocionante!");
        }
    }

    [TestClass]
    public class AddMatchesTaskTest
    {
        private readonly AddMatchesTask _task;

        public AddMatchesTaskTest()
        {
            var provider = new DataContextProvider();
            var cache = new Mock<ICache>();

            _task = new AddMatchesTask(provider, cache.Object);
        }

        [TestMethod]
        public async Task Run()
        {
            await _task.RunAsync();
        }
    }

    [TestClass]
    public class BonusExpirationWarningTaskTest
    {
        private readonly BonusExpirationWarningTask _task;
        private readonly Mock<IMailService> _mailMock = new Mock<IMailService>();

        public BonusExpirationWarningTaskTest()
        {
            var provider = new DataContextProvider();

            _task = new BonusExpirationWarningTask(_mailMock.Object, DateTime.UtcNow.AddHours(3).BrazilTimeZone(), provider);
        }

        [TestMethod]
        public void Run()
        {
            _task.Run();
        }
    }
}