using System;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.TestKit.Grains;

namespace Orleans
{
    public static class TestKitGrainExtensions
    {
        public static Task FireReminderAsync(this IAddressable grain, string reminderName, TickStatus status)
        {
            if (grain == null)
            {
                throw new ArgumentNullException(nameof(grain));
            }

            if (reminderName == null)
            {
                throw new ArgumentNullException(nameof(reminderName));
            }

            if (reminderName.Length == 0)
            {
                throw new ArgumentException("The reminder name must not be empty.", nameof(reminderName));
            }

            var reminderGrain = grain.AsReference<ITestKitReminderGrainExtension>();
            return reminderGrain.FireReminderAsync(reminderName, status);
        }
    }
}
