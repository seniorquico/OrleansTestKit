using System.Threading.Tasks;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    internal interface ITestKitBridgeGrain : IGrainWithGuidKey
    {
        Task<Immutable<TestKitReminder[]>> GetAllRemindersAsync(IAddressable grain);

        Task<Immutable<TestKitTimer[]>> GetAllTimersAsync(IAddressable grain);

        Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName)
            where TGrainState : new();

        Task<TestKitReminder> GetReminderAsync(IAddressable grain, string reminderName);

        Task<TestKitTimer> GetTimerAsync(IAddressable grain, int timerId);

        Task<string> SetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName, TGrainState state)
                            where TGrainState : new();
    }
}
