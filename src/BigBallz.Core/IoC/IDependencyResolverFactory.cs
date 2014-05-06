namespace BigBallz.Core.IoC
{
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}