using System;
using Moq;
using Orleans.Core;

namespace Orleans.TestKit
{
    public static class GrainProbeExtensions
    {
        #region Probes

        public static Mock<T> AddProbe<T>(this TestKitSilo silo, long id, string classPrefix = null) where T : class, IGrain
            => silo.GrainFactory.AddProbe<T>(new TestGrainIdentity(id), classPrefix);

        public static Mock<T> AddProbe<T>(this TestKitSilo silo, Guid id, string classPrefix = null) where T : class, IGrain
            => silo.GrainFactory.AddProbe<T>(new TestGrainIdentity(id), classPrefix);

        public static Mock<T> AddProbe<T>(this TestKitSilo silo, string id, string classPrefix = null) where T : class, IGrain
            => silo.GrainFactory.AddProbe<T>(new TestGrainIdentity(id), classPrefix);

        public static void AddProbe<T>(this TestKitSilo silo, Func<IGrainIdentity, IMock<T>> factory) where T : class, IGrain
            => silo.GrainFactory.AddProbe<T>(factory);
            

        #endregion Probes
    }
}