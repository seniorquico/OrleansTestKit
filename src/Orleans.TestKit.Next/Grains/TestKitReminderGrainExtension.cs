using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    internal sealed class TestKitReminderGrainExtension : Grain, ITestKitReminderGrainExtension
    {
        private readonly IGrainActivationContext context;

        public TestKitReminderGrainExtension(IGrainActivationContext? context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        public Task FireReminderAsync(string reminderName, TickStatus status) =>
            this.context.GrainInstance is IRemindable remindable
                ? remindable.ReceiveReminder(reminderName, status)
                : Task.CompletedTask;
    }
}
