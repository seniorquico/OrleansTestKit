using System;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Runtime.Services;

namespace Orleans.NewTestKit
{
    internal sealed class TestKitServiceClient : GrainServiceClient<ITestKitService>, ITestKitServiceClient
    {
        public TestKitServiceClient(IServiceProvider services)
            : base(services)
        {
        }

        public Task GetGrainReminderAsync(IAddressable grain, string reminderName) =>
            this.GrainService.GetGrainReminderAsync(grain, reminderName);

        public Task GetGrainRemindersAsync(IAddressable grain) =>
            this.GrainService.GetGrainRemindersAsync(grain);

        public Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName)
            where TGrainState : new() =>
            this.GrainService.GetGrainStateAsync<TGrainState>(grain, stateName, storageName);

        public Task GetGrainTimersAsync(IAddressable grain) =>
            this.GrainService.GetGrainTimersAsync(grain);
    }
}
