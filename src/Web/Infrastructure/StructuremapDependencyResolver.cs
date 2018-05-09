using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core.Bootstrapper;
using BigBallz.Core.Helper;
using BigBallz.Core.IoC;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace BigBallz.Infrastructure
{
    public class StructureMapDependecyResolver : IDependencyResolver
    {
        private IContainer _container;

        public StructureMapDependecyResolver()
        {
            Bootstrap();
        }

        private void Bootstrap()
        {
            _container = new Container(c => c.Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory();
                scan.LookForRegistries();
                scan.AddAllTypesOf<IBootstrapperTask>();
            }));
        }

        public void Dispose()
        {
            HttpContextLifecycle.DisposeAndClearAll();

            var disposable = (IDisposable)_container;

            disposable?.Dispose();
        }

        public void Register<T>(T instance) where T : class
        {
            Check.Argument.IsNotNull(instance, nameof(instance));

            _container.Configure(x => x.For<T>().AddInstances(y => y.Object(instance)));
        }

        public void Register<T>(Type type)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            _container.Configure(x => x.For(typeof(T)).Add(type));
        }

        public void Inject<T>(T existing) where T : class
        {
            _container.Inject(existing);
        }

        public T Resolve<T>(Type type)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            return (T)_container.GetInstance(type);
        }

        public T Resolve<T>(Type type, string name)
        {
            Check.Argument.IsNotNull(type, nameof(type));
            Check.Argument.IsNotEmpty(name, nameof(name));

            return (T)_container.TryGetInstance(type, name);
        }

        public T Resolve<T>()
        {
            return (T)_container.GetInstance(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, nameof(name));

            return (T)_container.TryGetInstance(typeof(T), name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.GetAllInstances(typeof(T)).OfType<T>();
        }

        public string Debug()
        {
            return _container.WhatDoIHave();
        }
    }
}