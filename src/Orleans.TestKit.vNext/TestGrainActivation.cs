using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Core;
using Orleans.GrainDirectory;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    public interface IColorGrain : IGrainWithIntegerKey
    {
        Task<string> GetAsync();

        Task SetAsync(string color);
    }

    public sealed class ColorGrain : Grain, IColorGrain
    {
        private string color = "";

        public Task<string> GetAsync() =>
            Task.FromResult(this.color);

        public Task SetAsync(string color)
        {
            this.color = color ?? "";
            return Task.CompletedTask;
        }
    }

    public sealed class Example
    {
        public async Task Test()
        {
            using (var activation = TestActivationBuilder
                .CreateBuilder<ColorGrain>(12345)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IList<int>>(new List<int>());
                })
                .Build())
            {
                var grain = activation.GrainInstance;
            }
        }
    }

    public interface ITestActivation<T> : IDisposable
        where T : Grain
    {
        T GrainInstance { get; set; }
    }

    public interface ITestActivationBuilder<T> where T : Grain
    {
        ITestActivation<T> Build();
    }

    public sealed class TestActivationBuilderImpl<T> : ITestActivationBuilder<T> where T : Grain
    {
        public ITestActivation<T> Build()
        {
            return null;
        }
    }

    public static class TestActivationBuilder
    {
        public static ITestActivationBuilder<T> CreateBuilder<T>(Guid key, string keyExt) where T : Grain, IGrainWithGuidCompoundKey
        {
            return null;
        }

        public static ITestActivationBuilder<T> CreateBuilder<T>(Guid key) where T : Grain, IGrainWithGuidKey
        {
            return null;
        }

        public static ITestActivationBuilder<T> CreateBuilder<T>(long key, string keyExt) where T : Grain, IGrainWithIntegerCompoundKey
        {
            return null;
        }

        public static ITestActivationBuilder<T> CreateBuilder<T>(long key) where T : Grain, IGrainWithIntegerKey
        {
            return null;
        }

        public static ITestActivationBuilder<T> CreateBuilder<T>(string key) where T : Grain, IGrainWithStringKey
        {
            return null;
        }
    }

    public static class TestActivationBuilderExtensions
    {
        public static ITestActivationBuilder<T> ConfigureServices<T>(this ITestActivationBuilder<T> builder, Action<IServiceCollection> configure)
            where T : Grain
        {
            configure(null);
            return builder;
        }
    }

    public sealed class Activation<T> where T : Grain
    {
        public Activation(T grain)
        {
            this.Grain = grain ?? throw new ArgumentNullException(nameof(grain));
        }

        public T Grain { get; }
    }

    public sealed class TestGrainActivation<T> where T : Grain
    {
        private readonly ServiceCollection serviceCollection;

        public TestGrainActivation(IServiceCollection serviceCollection)
        {
            this.serviceCollection = new ServiceCollection() ?? throw new ArgumentNullException(nameof(serviceCollection));
        }
    }

    public sealed class TestGrainActivationContext : IGrainActivationContext
    {
        public IServiceProvider ActivationServices =>
            throw new NotImplementedException();

        public IGrainIdentity GrainIdentity =>
            throw new NotImplementedException();

        public Grain GrainInstance =>
            throw new NotImplementedException();

        public Type GrainType =>
            throw new NotImplementedException();

        public IDictionary<object, object> Items =>
            throw new NotImplementedException();

        public IGrainLifecycle ObservableLifecycle =>
            throw new NotImplementedException();

        public IMultiClusterRegistrationStrategy RegistrationStrategy =>
            throw new NotImplementedException();
    }
}
