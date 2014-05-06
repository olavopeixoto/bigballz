using BigBallz.Core.IoC;

namespace BigBallz.Core.Bootstrapper
{
    public static class Bootstrapper
    {
        static Bootstrapper()
        {
            ServiceLocator.InitializeWith(new DependencyResolverFactory());
        }

        public static void Run()
        {
            ServiceLocator.ResolveAll<IBootstrapperTask>().ForEach(t => t.Execute());
        }
    }
}
