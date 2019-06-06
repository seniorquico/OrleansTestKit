using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Moq.Internals;
using Moq.Language;
using Moq.Language.Flow;
using Moq.Protected;
using Orleans;
using Orleans.Core;
using Orleans.GrainDirectory;
using Orleans.Runtime;
using Xunit;

namespace OrleansTestKit
{
    public sealed class ExampleTest
    {
        [Fact]
        public void Test()
        {
            var services = new ServiceCollection();

            // This exposes the GrainCreator API.
            services.TryAddSingleton(sp =>
            {
                var grainCreator = sp.GetRequiredService(GrainCreator.GrainCreatorType);
                return new GrainCreator(grainCreator);
            });

            // These are legit implementations.
            services.TryAddSingleton(GrainCreator.GrainCreatorType);
            services.TryAddSingleton<IGrainActivator, DefaultGrainActivator>();
            services.TryAddSingleton<Factory<IGrainRuntime>>(sp => () => sp.GetRequiredService<IGrainRuntime>());

            services.TryAddSingleton<IGrainRuntime>(sp => new Mock<IGrainRuntime>().Object);
            var providers = services.BuildServiceProvider();

            GrainId
        }

        private sealed class GrainActivationContext : IDisposable, IGrainActivationContext
        {
            private static readonly Type GrainLifecycleType = typeof(Silo).Assembly.GetType("Orleans.Runtime.GrainLifecycle");

            private readonly IServiceScope scope;

            public GrainActivationContext(IServiceProvider services, Type grainType, IGrainIdentity grainIdentity)
            {
                if (services == null)
                {
                    throw new ArgumentNullException(nameof(services));
                }

                this.scope = services.CreateScope();

                this.ActivationServices = this.scope.ServiceProvider;
                this.GrainIdentity = grainIdentity ?? throw new ArgumentNullException(nameof(grainIdentity));
                this.GrainInstance = null;
                this.GrainType = grainType ?? throw new ArgumentNullException(nameof(grainType));
                this.Items = new Dictionary<object, object>();
                this.ObservableLifecycle = (IGrainLifecycle)Activator.CreateInstance(GrainLifecycleType, new object[] { null });
                this.RegistrationStrategy = SingleInstanceRegistration.Singleton;
            }

            public IServiceProvider ActivationServices { get; }

            public IGrainIdentity GrainIdentity { get; }

            public Grain GrainInstance { get; }

            public Type GrainType { get; }

            public IDictionary<object, object> Items { get; }

            public IGrainLifecycle ObservableLifecycle { get; }

            public IMultiClusterRegistrationStrategy RegistrationStrategy { get; }

            public void Dispose() =>
                this.scope.Dispose();

            internal class SingleInstanceRegistration : IMultiClusterRegistrationStrategy
            {
                internal static SingleInstanceRegistration Singleton { get; } = new SingleInstanceRegistration();

                public override bool Equals(object obj) =>
                    obj is SingleInstanceRegistration;

                public override int GetHashCode() =>
                    this.GetType().GetHashCode();

                public IEnumerable<string> GetRemoteInstances(IReadOnlyList<string> clusters, string myClusterId) =>
                    Enumerable.Empty<string>();
            }
        }

        private sealed class GrainCreator
        {
            public static readonly Type GrainCreatorType = typeof(Silo).Assembly.GetType("Orleans.Runtime.GrainCreator");
            private static readonly MethodInfo CreateGrainInstanceMethod = GrainCreatorType.GetMethod("CreateGrainInstance", BindingFlags.Public | BindingFlags.Instance);
            private static readonly MethodInfo ReleaseMethod = GrainCreatorType.GetMethod("Release", BindingFlags.Public | BindingFlags.Instance);

            private readonly object grainCreator;

            public GrainCreator(object grainCreator) =>
                this.grainCreator = grainCreator ?? throw new ArgumentNullException(nameof(grainCreator));

            public Grain CreateGrainInstance(IGrainActivationContext context) =>
                (Grain)CreateGrainInstanceMethod.Invoke(this.grainCreator, new[] { context });

            public void Release(IGrainActivationContext context, object grain) =>
                ReleaseMethod.Invoke(this.grainCreator, new[] { context, grain });
        }
    }
}
