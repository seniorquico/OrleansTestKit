using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Services;

namespace Orleans.NewTestKit
{
    internal interface ITestKitService : IGrainService
    {
        Task GetGrainReminderAsync(IAddressable grain, string reminderName);

        Task GetGrainRemindersAsync(IAddressable grain);

        /// <summary>Asynchronously gets persistent state for the specified grain.</summary>
        /// <typeparam name="TGrainState">The type of the persistent state.</typeparam>
        /// <param name="grain">The grain reference.</param>
        /// <param name="stateName">
        ///     The name of the storage facet. Use <c>null</c> for grains that extend <see cref="Grain{TGrainState}"/>.
        /// </param>
        /// <param name="storageName">
        ///     The name of the storage provider. Use <c>null</c> for the default storage provider.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result returns a value tuple containing the
        ///     persistent state value and ETag.
        /// </returns>
        Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(IAddressable grain, string stateName, string storageName)
            where TGrainState : new();

        Task GetGrainTimersAsync(IAddressable grain);
    }
}
