using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.Utilities;

namespace Orleans.TestKit
{
    /// <summary>A grain extension that provides methods to get persistent state. This supports</summary>
    internal sealed class TestKitStorageExtension : ITestKitStorageExtension
    {
        private readonly IGrainActivationContext context;

        public TestKitStorageExtension(IGrainActivationContext context, IGrainRuntime runtime) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        public Task<(TGrainState state, string etag)> GetGrainFacetStateAsync<TGrainState>(string stateName, string storageName = null)
            where TGrainState : new()
        {
            // https://github.com/dotnet/orleans/blob/v2.3.0/src/Orleans.Runtime/Facet/Persistent/PersistentStateStorageFactory.cs#L29-L32
            var fullStateName = $"{RuntimeTypeNameFormatter.Format(this.context.GrainType)}.{stateName}";
            return this.GetStateFromStorageAsync<TGrainState>(fullStateName, storageName);
        }

        public Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(string storageName = null)
            where TGrainState : new()
        {
            // https://github.com/dotnet/orleans/blob/v2.3.0/src/Orleans.Runtime/Core/GrainRuntime.cs#L98-L99
            var fullStateName = this.context.GrainType.FullName;
            return this.GetStateFromStorageAsync<TGrainState>(fullStateName, storageName);
        }

        private async Task<(TGrainState state, string etag)> GetStateFromStorageAsync<TGrainState>(string fullStateName, string storageName = null)
            where TGrainState : new()
        {
            var grainStorage = storageName == null
                ? this.context.ActivationServices.GetService<IGrainStorage>()
                : this.context.ActivationServices.GetServiceByName<IGrainStorage>(storageName);
            if (grainStorage == null)
            {
                throw new ArgumentException("The specified storage provider does not exist.", nameof(storageName));
            }

            var storage = new StateStorageBridge<TGrainState>(
                fullStateName,
                this.context.GrainInstance.GrainReference,
                grainStorage,
                this.context.ActivationServices.GetService<ILoggerFactory>());
            await storage.ReadStateAsync();
            return (storage.State, storage.Etag);
        }
    }
}
