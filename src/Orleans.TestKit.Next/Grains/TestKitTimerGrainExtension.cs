using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit.Grains
{
    internal sealed class TestKitTimerGrainExtension : Grain, ITestKitTimerGrainExtension
    {
        private readonly IGrainActivationContext context;

        public TestKitTimerGrainExtension(IGrainActivationContext? context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        public Task FireTimerAsync(int timerId) =>
            this.context.GrainInstance is IRemindable remindable
                ? remindable.ReceiveReminder("Timer", new TickStatus())
                : Task.CompletedTask;
    }
}
