using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    /// <summary>Represents a grain extension that can fire grain reminders.</summary>
    internal interface ITestKitReminderGrainExtension : IGrainExtension
    {
        /// <summary>Asynchronously fires the reminder with the specified name.</summary>
        /// <param name="reminderName">The reminder name.</param>
        /// <param name="status">
        ///     The status of the reminder "tick". See <see cref="TickStatus"/> for more details.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task FireReminderAsync(string reminderName, TickStatus status);
    }
}
