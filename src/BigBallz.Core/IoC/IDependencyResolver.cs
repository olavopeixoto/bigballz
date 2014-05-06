﻿using System;
using System.Collections.Generic;

namespace BigBallz.Core.IoC
{
    public interface IDependencyResolver : IDisposable
    {
        void Register<T>(T instance);

        void Register<T>(Type type);

        void Inject<T>(T existing);

        T Resolve<T>(Type type);

        T Resolve<T>(Type type, string name);

        T Resolve<T>();

        T Resolve<T>(string name);

        IEnumerable<T> ResolveAll<T>();
    }
}