using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using Orleans.Runtime;
using Orleans;

namespace OrleansTestKit.Runtime
{
    public static class Context
    {
        public static void Setup(IGrainActivationContext context)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.TryAddSingleton(GrainCreatorType.Value); // .TryAddSingleton<GrainCreator>()
            serviceCollection.TryAddSingleton<IGrainActivator, DefaultGrainActivator>();
            serviceCollection.TryAddSingleton<IGrainRuntime, TestGrainRuntime>();

            // var serviceProvider = serviceCollection.BuildServiceProvider(); var createGrainInstance =
            // GrainCreatorCreateGrainInstanceMethod.Value; var parameters = new object[] { context };
            // createGrainInstance.Invoke(grainCreator, parameters);
            // GrainCreatorType.Value.GetMethod("CreateGrainInstance", BindingFlags.Instance | BindingFlags.Public);
        }

        private static object CreateGrainCreator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var grainActivator = serviceProvider.GetRequiredService<IGrainActivator>(); // DefaultGrainActivator
            var getGrainRuntime
            Factory<IGrainRuntime> getGrainRuntime = () => new TestGrainRuntime(serviceProvider);
            return Activator.CreateInstance(
                GrainCreatorType.Value,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { grainActivator, getGrainRuntime },
                CultureInfo.InvariantCulture);
        }
    }

    public sealed class Fixture : IDisposable
    {
        private static readonly Lazy<MethodInfo> GrainCreatorMethodCreateGrainInstance = new Lazy<MethodInfo>(
            GetGrainCreatorMethodCreateGrainInstance,
            LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly Lazy<MethodInfo> GrainCreatorMethodRelease = new Lazy<MethodInfo>(
            GetGrainCreatorMethodRelease,
            LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly Lazy<Type> GrainCreatorType = new Lazy<Type>(
            GetGrainCreatorType,
            LazyThreadSafetyMode.ExecutionAndPublication);

        private TestGrainActivationContext context;
        private IServiceProvider services;

        public Grain Activate(Type grainType, IServiceCollection serviceDescriptors = null)
        {
            if (grainType == null)
            {
                throw new ArgumentNullException(nameof(grainType));
            }

            if (serviceDescriptors == null)
            {
                serviceDescriptors = new ServiceCollection();
            }

            serviceDescriptors.TryAddSingleton(GrainCreatorType.Value); // .TryAddSingleton<GrainCreator>()
            serviceDescriptors.TryAddSingleton<IGrainActivator, DefaultGrainActivator>();
            serviceDescriptors.TryAddSingleton<IGrainRuntime, TestGrainRuntime>();
            this.services = serviceDescriptors.BuildServiceProvider();

            this.context = new TestGrainActivationContext(grainType, this.services);

            var grainCreator = this.services.GetRequiredService(GrainCreatorType.Value);
            var grain = GrainCreatorMethodCreateGrainInstance.Value.Invoke(grainCreator, new object[] { this.context });
            this.context.GrainInstance = (Grain)grain;

            // TODO: Set grain.Data

            return this.context.GrainInstance;
        }

        public void Deactivate()
        {
        }

        public void Dispose()
        {
            if (this.grainCreator != null)
            {
                GrainCreatorMethodRelease.Value.Invoke(this.grainCreator, new object[0]);
                this.grainCreator = null;
            }
        }

        private static MethodInfo GetGrainCreatorMethodCreateGrainInstance()
        {
            var type = GrainCreatorType.Value;
            var method = type.GetMethod("CreateGrainInstance", BindingFlags.Public | BindingFlags.Instance);
            return method;
        }

        private static MethodInfo GetGrainCreatorMethodRelease()
        {
            var type = GrainCreatorType.Value;
            var method = type.GetMethod("Release", BindingFlags.Public | BindingFlags.Instance);
            return method;
        }

        private static Type GetGrainCreatorType()
        {
            var assembly = typeof(DefaultGrainActivator).Assembly;
            var type = assembly.GetType("Orleans.Runtime.GrainCreator");
            return type;
        }
    }
}
