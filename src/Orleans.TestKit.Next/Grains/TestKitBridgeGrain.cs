using System;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    internal sealed class TestKitBridgeGrain : Grain, ITestKitBridgeGrain
    {
        public Task<Immutable<TestKitReminder[]>> GetAllRemindersAsync(IAddressable grain) => throw new NotImplementedException();

        public Task<Immutable<TestKitTimer[]>> GetAllTimersAsync(IAddressable grain) => throw new NotImplementedException();

        public Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName)
            where TGrainState : new() =>
            throw new NotImplementedException();

        public Task<TestKitReminder> GetReminderAsync(IAddressable grain, string reminderName) => throw new NotImplementedException();

        public Task<TestKitTimer> GetTimerAsync(IAddressable grain, int timerId) => throw new NotImplementedException();

        public Task<string> SetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName, TGrainState state)
            where TGrainState : new() =>
            throw new NotImplementedException();
    }
}
