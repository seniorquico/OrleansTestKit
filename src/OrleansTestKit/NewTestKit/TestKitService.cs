using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Core;
using Orleans.Runtime;

namespace Orleans.NewTestKit
{
    internal sealed class TestKitService : GrainService, ITestKitService
    {
        private readonly IGrainFactory grainFactory;
        private readonly IServiceProvider services;

        public TestKitService(IGrainIdentity id, Silo silo, ILoggerFactory loggerFactory, IGrainFactory grainFactory, IServiceProvider services)
            : base(id, silo, loggerFactory)
        {
            this.grainFactory = grainFactory ?? throw new ArgumentNullException(nameof(grainFactory));
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public Task GetGrainReminderAsync(IAddressable grain, string reminderName) =>
            throw new NotImplementedException();

        public Task GetGrainRemindersAsync(IAddressable grain) =>
            throw new NotImplementedException();

        public Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName)
            where TGrainState : new() =>
            throw new NotImplementedException();

        public Task GetGrainTimerAsync(IAddressable grain, string timerName) =>
            throw new NotImplementedException();

        public Task GetGrainTimersAsync(IAddressable grain) => throw new NotImplementedException();
    }
}
