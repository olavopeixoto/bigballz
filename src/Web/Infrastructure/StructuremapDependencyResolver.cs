using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core.Bootstrapper;
using BigBallz.Core.Helper;
using BigBallz.Core.IoC;
using StructureMap;

namespace BigBallz.Infrastructure
{
    public class StructureMapDependecyResolver : IDependencyResolver
    {
        public StructureMapDependecyResolver()
        {
            Bootstrap();
        }

        private void Bootstrap()
        {
            ObjectFactory.Initialize((x =>
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.LookForRegistries();
                    scan.AddAllTypesOf<IBootstrapperTask>();
                })));
        }

        public void Dispose()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();

            var disposable = (IDisposable)ObjectFactory.Container;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public void Register<T>(T instance)
        {
            Check.Argument.IsNotNull(instance, "instance");
            ObjectFactory.Configure(x => x.For<T>().AddInstances(y => y.Object(instance)));
        }

        public void Register<T>(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            ObjectFactory.Configure(x => x.For(typeof(T)).Add(type));
        }

        public void Inject<T>(T existing)
        {
            ObjectFactory.Inject(existing);
        }

        public T Resolve<T>(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            return (T)ObjectFactory.GetInstance(type);
        }

        public T Resolve<T>(Type type, string name)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");

            return (T)ObjectFactory.TryGetInstance(type, name);
        }

        public T Resolve<T>()
        {
            return (T)ObjectFactory.GetInstance(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return (T)ObjectFactory.TryGetInstance(typeof(T), name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return ObjectFactory.GetAllInstances(typeof(T)).OfType<T>();
        }
    }
}