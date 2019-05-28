using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    /// <summary>Represents a grain extension that can fire grain timers.</summary>
    internal interface ITestKitTimerGrainExtension : IGrainExtension
    {
        /// <summary>Asynchronously fires the timer with the specified identifier.</summary>
        /// <param name="timerId">The timer identifier.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task FireTimerAsync(int timerId);
    }
}
