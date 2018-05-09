using System;
using System.Collections.Generic;
using System.Diagnostics;
using BigBallz.Core.Helper;

namespace BigBallz.Core.IoC
{
    public static class ServiceLocator
    {
        private static IDependencyResolver _resolver;

        [DebuggerStepThrough]
        public static void InitializeWith(IDependencyResolverFactory factory)
        {
            Check.Argument.IsNotNull(factory, nameof(factory));

            _resolver = factory.CreateInstance();
        }

        [DebuggerStepThrough]
        public static void Register<T>(T instance) where T : class
        {
            Check.Argument.IsNotNull(instance, nameof(instance));

            _resolver.Register(instance);
        }

        [DebuggerStepThrough]
        public static void Register<T>(Type concreteType)
        {
            Check.Argument.IsNotNull(concreteType, nameof(concreteType));

            _resolver.Register<T>(concreteType);
        }

        [DebuggerStepThrough]
        public static void Inject<T>(T existing) where T : class
        {
            Check.Argument.IsNotNull(existing, nameof(existing));

            _resolver.Inject(existing);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            return _resolver.Resolve<T>(type);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type, string name)
        {
            Check.Argument.IsNotNull(type, nameof(type));
            Check.Argument.IsNotEmpty(name, nameof(name));

            return _resolver.Resolve<T>(type, name);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, nameof(name));

            return _resolver.Resolve<T>(name);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ResolveAll<T>()
        {
            return _resolver.ResolveAll<T>();
        }

        [DebuggerStepThrough]
        public static void Reset()
        {
            _resolver?.Dispose();
        }

        [DebuggerStepThrough]
        public static string Debug()
        {
            return _resolver.Debug();
        }
    }
}