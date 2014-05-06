using System;

namespace BigBallz.Services
{
    public interface ICronJobTask
    {
        string Name { get; }
        bool Recurring { get; }
        DateTime? AbsoluteExpiration { get; }
        TimeSpan? SlidingExpiration { get; }
        void Run();
    }
}
